using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Models.ViewModels;
using MarketLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

[Authorize(Roles = "Customer")]
public class CustomerController : Controller
{
    private readonly MarketLinkContext _db;
    private readonly IAuthService _auth;
    private readonly NotificationService _notify;

    public CustomerController(MarketLinkContext db, IAuthService auth, NotificationService notify)
    {
        _db = db;
        _auth = auth;
        _notify = notify;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _auth.CurrentUser(User);
        if (user == null) return RedirectToAction("Login", "Account");

        ViewBag.ActiveOrders = await _db.Orders
            .Include(o => o.Farmer)
            .Where(o => o.CustomerId == user.Id &&
                        (o.Status == OrderStatus.Placed || o.Status == OrderStatus.Accepted || o.Status == OrderStatus.ReadyForPickup))
            .OrderBy(o => o.PickupDate)
            .ToListAsync();

        ViewBag.PastOrders = await _db.Orders
            .Include(o => o.Farmer)
            .Where(o => o.CustomerId == user.Id && o.Status == OrderStatus.Completed)
            .OrderByDescending(o => o.PickupDate)
            .Take(5)
            .ToListAsync();

        ViewBag.Favorites = await _db.Favorites
            .Include(f => f.Product).ThenInclude(p => p!.Farmer)
            .Include(f => f.Farmer)
            .Where(f => f.CustomerId == user.Id)
            .ToListAsync();

        ViewBag.Notifications = await _db.Notifications
            .Where(n => n.UserId == user.Id)
            .OrderByDescending(n => n.CreatedAt)
            .Take(6)
            .ToListAsync();

        ViewBag.UnreadCount = await _db.Notifications.CountAsync(n => n.UserId == user.Id && !n.IsRead);

        // Restock alerts — favourited products that are back in stock
        ViewBag.RestockItems = await _db.Favorites
            .Include(f => f.Product).ThenInclude(p => p!.Farmer)
            .Where(f => f.CustomerId == user.Id && f.Product != null &&
                        f.Product.IsAvailable && !f.Product.IsSoldOut && f.Product.StockQuantity > 0)
            .Select(f => f.Product!)
            .Take(4)
            .ToListAsync();

        return View();
    }

    // ───────────────────────── Favorites ─────────────────────────
    public async Task<IActionResult> Favorites()
    {
        var user = await _auth.CurrentUser(User);
        var favorites = await _db.Favorites
            .Include(f => f.Product).ThenInclude(p => p!.Farmer)
            .Include(f => f.Product).ThenInclude(p => p!.Category)
            .Include(f => f.Farmer)
            .Where(f => f.CustomerId == user!.Id)
            .ToListAsync();
        return View(favorites);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFavorite(int? productId, int? farmerId, string? returnUrl = null)
    {
        var user = await _auth.CurrentUser(User);
        if (user == null) return RedirectToAction("Login", "Account");
        if (productId == null && farmerId == null) return RedirectToAction(nameof(Favorites));

        var existing = await _db.Favorites.FirstOrDefaultAsync(f =>
            f.CustomerId == user.Id &&
            (productId != null ? f.ProductId == productId : f.FarmerId == farmerId));

        if (existing != null)
        {
            _db.Favorites.Remove(existing);
            TempData["Info"] = "Removed from favourites.";
        }
        else
        {
            _db.Favorites.Add(new Favorite
            {
                CustomerId = user.Id,
                ProductId = productId,
                FarmerId = productId == null ? farmerId : null
            });
            TempData["Success"] = "Saved to favourites ♡";
        }
        await _db.SaveChangesAsync();

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction(nameof(Favorites));
    }

    // ───────────────────────── Reviews ─────────────────────────
    public async Task<IActionResult> Reviews()
    {
        var user = await _auth.CurrentUser(User);
        var reviews = await _db.Reviews
            .Include(r => r.Product).ThenInclude(p => p!.Farmer)
            .Where(r => r.CustomerId == user!.Id)
            .OrderByDescending(r => r.ReviewDate)
            .ToListAsync();
        return View(reviews);
    }

    [HttpGet]
    public async Task<IActionResult> WriteReview(int productId, int orderId)
    {
        var user = await _auth.CurrentUser(User);
        var order = await _db.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == user!.Id && o.Status == OrderStatus.Completed);
        if (order == null || order.Items.All(i => i.ProductId != productId)) return NotFound();

        var product = await _db.Products.FindAsync(productId);
        if (product == null) return NotFound();

        return View(new ReviewFormViewModel
        {
            ProductId = productId,
            OrderId = orderId,
            ProductName = product.Name
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> WriteReview(ReviewFormViewModel model)
    {
        var user = await _auth.CurrentUser(User);
        var product = await _db.Products.FindAsync(model.ProductId);
        if (product == null || user == null) return NotFound();

        if (!ModelState.IsValid)
        {
            model.ProductName = product.Name;
            return View(model);
        }

        _db.Reviews.Add(new Review
        {
            ProductId = model.ProductId,
            CustomerId = user.Id,
            FarmerId = product.FarmerId,
            Rating = model.Rating,
            Comment = model.Comment
        });
        await _db.SaveChangesAsync();

        await _notify.NotifyAsync(product.FarmerId,
            "New review received",
            $"{user.FullName} rated {product.Name} {model.Rating}/5.",
            "Review", "/Farmer/Reviews");

        TempData["Success"] = "Thank you — your review has been published.";
        return RedirectToAction(nameof(Reviews));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var user = await _auth.CurrentUser(User);
        var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == id && r.CustomerId == user!.Id);
        if (review != null)
        {
            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Review removed.";
        }
        return RedirectToAction(nameof(Reviews));
    }

    // ───────────────────────── Notifications ─────────────────────────
    public async Task<IActionResult> Notifications()
    {
        var user = await _auth.CurrentUser(User);
        var items = await _db.Notifications
            .Where(n => n.UserId == user!.Id)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync();
        return View(items);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkNotificationsRead()
    {
        var user = await _auth.CurrentUser(User);
        var unread = await _db.Notifications.Where(n => n.UserId == user!.Id && !n.IsRead).ToListAsync();
        foreach (var n in unread) n.IsRead = true;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Notifications));
    }
}
