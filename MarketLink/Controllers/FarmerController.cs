using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Models.ViewModels;
using MarketLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

[Authorize(Roles = "Farmer")]
public class FarmerController : Controller
{
    private readonly MarketLinkContext _db;
    private readonly IAuthService _auth;
    private readonly NotificationService _notify;

    public FarmerController(MarketLinkContext db, IAuthService auth, NotificationService notify)
    {
        _db = db;
        _auth = auth;
        _notify = notify;
    }

    private async Task<User?> RequireFarmerAsync()
    {
        var user = await _auth.CurrentUser(User);
        if (user == null || !user.IsActive) return null;
        return user;
    }

    // ───────────────────────── Dashboard & insights ─────────────────────────
    public async Task<IActionResult> Dashboard()
    {
        var farmer = await RequireFarmerAsync();
        if (farmer == null) return RedirectToAction("Login", "Account");
        if (!farmer.IsApproved)
        {
            ViewBag.PendingApproval = true;
            return View(new FarmerDashboardViewModel());
        }

        var orders = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .Where(o => o.FarmerId == farmer.Id)
            .ToListAsync();

        var completed = orders.Where(o => o.Status == OrderStatus.Completed).ToList();

        // Revenue trend over the last 7 days
        var revenueByDay = new List<decimal>();
        for (var i = 6; i >= 0; i--)
        {
            var day = DateTime.Today.AddDays(-i);
            revenueByDay.Add(completed
                .Where(o => o.PickupDate.Date == day)
                .Sum(o => o.TotalAmount));
        }

        var topProducts = orders
            .Where(o => o.Status == OrderStatus.Completed || o.Status == OrderStatus.Accepted || o.Status == OrderStatus.ReadyForPickup)
            .SelectMany(o => o.Items)
            .GroupBy(i => i.ProductName)
            .Select(g => new TopProductViewModel
            {
                Name = g.Key,
                UnitsSold = g.Sum(i => i.Quantity),
                Revenue = g.Sum(i => i.LineTotal)
            })
            .OrderByDescending(t => t.UnitsSold)
            .Take(5)
            .ToList();

        var vm = new FarmerDashboardViewModel
        {
            TotalOrders = orders.Count,
            PendingOrders = orders.Count(o => o.Status == OrderStatus.Placed),
            CompletedOrders = completed.Count,
            Revenue = completed.Sum(o => o.TotalAmount),
            RecentOrders = orders.OrderByDescending(o => o.OrderDate).Take(6).ToList(),
            TopProducts = topProducts,
            ProductCount = await _db.Products.CountAsync(p => p.FarmerId == farmer.Id),
            LowStockCount = await _db.Products.CountAsync(p => p.FarmerId == farmer.Id && p.StockQuantity <= 5 && p.IsAvailable),
            AverageRating = await _db.Reviews.Where(r => r.FarmerId == farmer.Id && r.IsVisible)
                .AverageAsync(r => (double?)r.Rating) ?? 0,
            RevenueByDay = revenueByDay
        };
        return View(vm);
    }

    // ───────────────────────── Products / weekly stock ─────────────────────────
    public async Task<IActionResult> Products()
    {
        var farmer = await RequireFarmerAsync();
        if (farmer == null) return RedirectToAction("Login", "Account");

        var products = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.FarmerId == farmer.Id)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        await LoadCategoriesAsync();
        return View("ProductForm", new ProductFormViewModel());
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var farmer = await RequireFarmerAsync();
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.FarmerId == farmer!.Id);
        if (product == null) return NotFound();

        await LoadCategoriesAsync();
        return View("ProductForm", new ProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Unit = product.Unit,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            IsAvailable = product.IsAvailable,
            IsSoldOut = product.IsSoldOut,
            IsRecurringWeekly = product.IsRecurringWeekly,
            DefaultWeeklyQuantity = product.DefaultWeeklyQuantity
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProduct(ProductFormViewModel model)
    {
        var farmer = await RequireFarmerAsync();
        if (farmer == null) return RedirectToAction("Login", "Account");
        if (!farmer.IsApproved)
        {
            TempData["Error"] = "Your stall must be approved before listing produce.";
            return RedirectToAction(nameof(Products));
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View("ProductForm", model);
        }

        Product product;
        if (model.Id == 0)
        {
            product = new Product { FarmerId = farmer.Id };
            _db.Products.Add(product);
            TempData["Success"] = "Produce listed on the market!";
        }
        else
        {
            product = await _db.Products.FirstOrDefaultAsync(p => p.Id == model.Id && p.FarmerId == farmer.Id)
                      ?? throw new InvalidOperationException("Product not found.");
            TempData["Success"] = "Produce updated.";

            // Restock alerts — notify favouriting customers when a sold-out item returns
            if (product.IsSoldOut && !model.IsSoldOut && model.StockQuantity > 0)
            {
                var fans = await _db.Favorites
                    .Where(f => f.ProductId == product.Id)
                    .Select(f => f.CustomerId)
                    .ToListAsync();
                await _notify.NotifyManyAsync(fans,
                    "Back in stock ♡",
                    $"{product.Name} is available again at {farmer.StallName ?? farmer.FullName}.",
                    "Restock", $"/Marketplace/Details/{product.Id}");
            }
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.Unit = model.Unit;
        product.StockQuantity = model.StockQuantity;
        product.ImageUrl = model.ImageUrl;
        product.CategoryId = model.CategoryId;
        product.IsAvailable = model.IsAvailable;
        product.IsSoldOut = model.IsSoldOut;
        product.IsRecurringWeekly = model.IsRecurringWeekly;
        product.DefaultWeeklyQuantity = model.DefaultWeeklyQuantity;
        product.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Products));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var farmer = await RequireFarmerAsync();
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.FarmerId == farmer!.Id);
        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Produce removed from the listing.";
        }
        return RedirectToAction(nameof(Products));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleSoldOut(int id)
    {
        var farmer = await RequireFarmerAsync();
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.FarmerId == farmer!.Id);
        if (product != null)
        {
            product.IsSoldOut = !product.IsSoldOut;
            if (product.IsSoldOut) product.StockQuantity = 0;
            else if (product.StockQuantity == 0) product.StockQuantity = product.DefaultWeeklyQuantity > 0 ? product.DefaultWeeklyQuantity : 10;
            product.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            TempData["Success"] = product.IsSoldOut ? "Marked as sold out." : "Back in stock!";
        }
        return RedirectToAction(nameof(Products));
    }

    /// <summary>Reset recurring items to their weekly template quantity.</summary>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetWeeklyStock()
    {
        var farmer = await RequireFarmerAsync();
        if (farmer == null) return RedirectToAction("Login", "Account");

        var items = await _db.Products
            .Where(p => p.FarmerId == farmer.Id && p.IsRecurringWeekly)
            .ToListAsync();

        foreach (var p in items)
        {
            p.StockQuantity = p.DefaultWeeklyQuantity;
            p.IsSoldOut = p.StockQuantity == 0;
            p.UpdatedAt = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();

        TempData["Success"] = $"Weekly stock refreshed for {items.Count} recurring item(s).";
        return RedirectToAction(nameof(Products));
    }

    private async Task LoadCategoriesAsync() =>
        ViewBag.Categories = await _db.Categories.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToListAsync();

    // ───────────────────────── Pre-order management ─────────────────────────
    public async Task<IActionResult> Orders(string? status)
    {
        var farmer = await RequireFarmerAsync();
        if (farmer == null) return RedirectToAction("Login", "Account");

        var query = _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .Where(o => o.FarmerId == farmer.Id);

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, out var s))
            query = query.Where(o => o.Status == s);

        ViewBag.CurrentStatus = status;
        ViewBag.StatusCounts = await _db.Orders
            .Where(o => o.FarmerId == farmer.Id)
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count);

        var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync();
        return View(orders);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(int id, string status, string? note = null)
    {
        var farmer = await RequireFarmerAsync();
        if (farmer == null) return RedirectToAction("Login", "Account");
        var order = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id && o.FarmerId == farmer!.Id);
        if (order == null) return NotFound();

        if (!Enum.TryParse<OrderStatus>(status, out var newStatus))
            return RedirectToAction(nameof(Orders));

        order.Status = newStatus;
        order.StatusUpdatedAt = DateTime.UtcNow;
        order.FarmerNote = note ?? order.FarmerNote;
        await _db.SaveChangesAsync();

        (string title, string message) = newStatus switch
        {
            OrderStatus.Accepted => ("Pre-order accepted",
                $"{farmer.StallName ?? farmer.FullName} accepted your order {order.OrderNumber} for pickup on {order.PickupDate:dddd, d MMM}."),
            OrderStatus.Declined => ("Pre-order declined",
                $"{farmer.StallName ?? farmer.FullName} could not fulfil order {order.OrderNumber}. Please browse their other produce."),
            OrderStatus.ReadyForPickup => ("Your basket is ready",
                $"Order {order.OrderNumber} is packed and waiting at the {farmer.StallName ?? farmer.FullName} stall ({order.PickupWindow})."),
            OrderStatus.Completed => ("Order completed",
                $"Order {order.OrderNumber} has been collected. We would love a review!"),
            _ => ("Order updated", $"Order {order.OrderNumber} is now {newStatus}.")
        };

        await _notify.NotifyAsync(order.CustomerId, title, message, "Order", "/PreOrder/Orders");

        // Declined orders return their reserved stock
        if (newStatus == OrderStatus.Declined)
        {
            foreach (var item in order.Items)
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product == null) continue;
                product.StockQuantity += item.Quantity;
                if (product.IsSoldOut && product.StockQuantity > 0) product.IsSoldOut = false;
            }
            await _db.SaveChangesAsync();
        }

        TempData["Success"] = $"Order {order.OrderNumber} marked {newStatus}.";
        return RedirectToAction(nameof(Orders));
    }

    // ───────────────────────── Reviews ─────────────────────────
    public async Task<IActionResult> Reviews()
    {
        var farmer = await RequireFarmerAsync();
        if (farmer == null) return RedirectToAction("Login", "Account");

        var reviews = await _db.Reviews
            .Include(r => r.Customer)
            .Include(r => r.Product)
            .Where(r => r.FarmerId == farmer.Id && r.IsVisible)
            .OrderByDescending(r => r.ReviewDate)
            .ToListAsync();
        return View(reviews);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RespondToReview(int id, string response)
    {
        var farmer = await RequireFarmerAsync();
        var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == id && r.FarmerId == farmer!.Id);
        if (review != null && !string.IsNullOrWhiteSpace(response))
        {
            review.FarmerResponse = response.Trim();
            review.ResponseDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Your response has been published.";
        }
        return RedirectToAction(nameof(Reviews));
    }
}
