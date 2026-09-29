using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Models.ViewModels;
using MarketLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly MarketLinkContext _db;
    private readonly IAuthService _auth;
    private readonly NotificationService _notify;

    public AdminController(MarketLinkContext db, IAuthService auth, NotificationService notify)
    {
        _db = db;
        _auth = auth;
        _notify = notify;
    }

    // ───────────────────────── Dashboard ─────────────────────────
    public async Task<IActionResult> Dashboard()
    {
        var completedOrders = await _db.Orders
            .Where(o => o.Status == OrderStatus.Completed)
            .Include(o => o.Items)
            .Include(o => o.Farmer).ThenInclude(f => f.MarketLinks).ThenInclude(mf => mf.Market)
            .ToListAsync();

        var revenueByMarket = completedOrders
            .SelectMany(o => o.Items.Select(i => new { Market = o.Farmer.MarketLinks.FirstOrDefault()?.Market.Name ?? "Unassigned", i.LineTotal }))
            .GroupBy(x => x.Market)
            .Select(g => new MarketRevenueViewModel { Market = g.Key, Revenue = g.Sum(x => x.LineTotal) })
            .OrderByDescending(m => m.Revenue)
            .ToList();

        var topProducts = completedOrders
            .SelectMany(o => o.Items)
            .GroupBy(i => i.ProductName)
            .Select(g => new TopProductViewModel { Name = g.Key, UnitsSold = g.Sum(i => i.Quantity), Revenue = g.Sum(i => i.LineTotal) })
            .OrderByDescending(t => t.Revenue)
            .Take(5)
            .ToList();

        var vm = new AdminDashboardViewModel
        {
            TotalFarmers = await _db.Users.CountAsync(u => u.Role == UserRole.Farmer && u.IsApproved),
            TotalCustomers = await _db.Users.CountAsync(u => u.Role == UserRole.Customer),
            TotalMarkets = await _db.Markets.CountAsync(m => m.IsActive),
            TotalOrders = await _db.Orders.CountAsync(),
            TotalRevenue = completedOrders.Sum(o => o.TotalAmount),
            PendingFarmerApprovals = await _db.Users.CountAsync(u => u.Role == UserRole.Farmer && !u.IsApproved),
            RecentOrders = await _db.Orders
                .Include(o => o.Customer).Include(o => o.Farmer)
                .OrderByDescending(o => o.OrderDate).Take(8).ToListAsync(),
            TopProducts = topProducts,
            RevenueByMarket = revenueByMarket
        };
        return View(vm);
    }

    // ───────────────────────── Farmer management ─────────────────────────
    public async Task<IActionResult> Farmers()
    {
        var farmers = await _db.Users
            .Where(u => u.Role == UserRole.Farmer)
            .Include(u => u.Products)
            .Include(u => u.MarketLinks).ThenInclude(mf => mf.Market)
            .OrderBy(u => u.IsApproved).ThenBy(u => u.FullName)
            .ToListAsync();
        return View(farmers);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveFarmer(int id, bool approve)
    {
        var farmer = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRole.Farmer);
        if (farmer != null)
        {
            farmer.IsApproved = approve;
            await _db.SaveChangesAsync();
            await _notify.NotifyAsync(farmer.Id,
                approve ? "Stall approved — welcome to the market!" : "Stall listing paused",
                approve
                    ? $"Your stall '{farmer.StallName}' is now live. You can list produce from your dashboard."
                    : "An administrator has paused your stall. Please contact the platform office.",
                "Account", "/Farmer/Dashboard");
            TempData["Success"] = approve ? $"{farmer.StallName} approved." : $"{farmer.StallName} suspended.";
        }
        return RedirectToAction(nameof(Farmers));
    }

    // ───────────────────────── Customer management ─────────────────────────
    public async Task<IActionResult> Customers()
    {
        var customers = await _db.Users
            .Where(u => u.Role == UserRole.Customer)
            .Include(u => u.Orders)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
        return View(customers);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleCustomer(int id)
    {
        var customer = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRole.Customer);
        if (customer != null)
        {
            customer.IsActive = !customer.IsActive;
            await _db.SaveChangesAsync();
            TempData["Success"] = $"{customer.FullName} is now {(customer.IsActive ? "active" : "deactivated")}.";
        }
        return RedirectToAction(nameof(Customers));
    }

    // ───────────────────────── Market management ─────────────────────────
    public async Task<IActionResult> Markets()
    {
        var markets = await _db.Markets
            .Include(m => m.FarmerLinks)
            .OrderBy(m => m.Name)
            .ToListAsync();
        return View(markets);
    }

    [HttpGet]
    public IActionResult CreateMarket() => View("MarketForm", new Market());

    [HttpGet]
    public async Task<IActionResult> EditMarket(int id)
    {
        var market = await _db.Markets.FindAsync(id);
        return market == null ? NotFound() : View("MarketForm", market);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveMarket(Market market)
    {
        if (!ModelState.IsValid) return View("MarketForm", market);

        if (market.Id == 0)
        {
            _db.Markets.Add(market);
            TempData["Success"] = "Market added.";
        }
        else
        {
            var existing = await _db.Markets.FindAsync(market.Id);
            if (existing == null) return NotFound();
            existing.Name = market.Name;
            existing.Description = market.Description;
            existing.Address = market.Address;
            existing.City = market.City;
            existing.Latitude = market.Latitude;
            existing.Longitude = market.Longitude;
            existing.OperatingDays = market.OperatingDays;
            existing.OpenTime = market.OpenTime;
            existing.CloseTime = market.CloseTime;
            existing.ImageUrl = market.ImageUrl;
            existing.IsActive = market.IsActive;
            existing.MapProvider = market.MapProvider;
            TempData["Success"] = "Market updated.";
        }
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Markets));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMarket(int id)
    {
        var market = await _db.Markets.FindAsync(id);
        if (market != null)
        {
            market.IsActive = false;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Market removed from listings.";
        }
        return RedirectToAction(nameof(Markets));
    }

    // ───────────────────────── Category management ─────────────────────────
    public async Task<IActionResult> Categories()
    {
        var categories = await _db.Categories
            .Include(c => c.Products)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();
        return View(categories);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCategory(int id, string name, string? description, string? imageUrl, int sortOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            TempData["Error"] = "Category name is required.";
            return RedirectToAction(nameof(Categories));
        }

        if (id == 0)
        {
            _db.Categories.Add(new Category { Name = name.Trim(), Description = description, ImageUrl = imageUrl, SortOrder = sortOrder });
            TempData["Success"] = "Category added.";
        }
        else
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat != null)
            {
                cat.Name = name.Trim();
                cat.Description = description;
                cat.ImageUrl = imageUrl;
                cat.SortOrder = sortOrder;
                TempData["Success"] = "Category updated.";
            }
        }
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Categories));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleCategory(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat != null)
        {
            cat.IsActive = !cat.IsActive;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Categories));
    }

    // ───────────────────────── Content moderation ─────────────────────────
    public async Task<IActionResult> Moderation()
    {
        ViewBag.Products = await _db.Products
            .Include(p => p.Farmer)
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .Take(30)
            .ToListAsync();

        ViewBag.Reviews = await _db.Reviews
            .Include(r => r.Customer)
            .Include(r => r.Product)
            .OrderByDescending(r => r.ReviewDate)
            .Take(30)
            .ToListAsync();

        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleProductVisibility(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            product.IsVisible = !product.IsVisible;
            await _db.SaveChangesAsync();
            TempData["Success"] = product.IsVisible ? "Listing restored." : "Listing hidden from the marketplace.";
        }
        return RedirectToAction(nameof(Moderation));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleReviewVisibility(int id)
    {
        var review = await _db.Reviews.FindAsync(id);
        if (review != null)
        {
            review.IsVisible = !review.IsVisible;
            await _db.SaveChangesAsync();
            TempData["Success"] = review.IsVisible ? "Review restored." : "Review hidden.";
        }
        return RedirectToAction(nameof(Moderation));
    }

    // ───────────────────────── Reports & analytics ─────────────────────────
    public async Task<IActionResult> Reports(DateTime? from, DateTime? to)
    {
        var start = from ?? DateTime.Today.AddMonths(-1);
        var end = (to ?? DateTime.Today).Date.AddDays(1);

        var orders = await _db.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .Include(o => o.Farmer).ThenInclude(f => f.MarketLinks).ThenInclude(mf => mf.Market)
            .Where(o => o.OrderDate >= start && o.OrderDate < end)
            .ToListAsync();

        var completed = orders.Where(o => o.Status == OrderStatus.Completed).ToList();

        var vm = new AdminReportViewModel
        {
            From = start,
            To = end.AddDays(-1),
            TotalOrders = orders.Count,
            Revenue = completed.Sum(o => o.TotalAmount),
            RevenueByMarket = completed
                .SelectMany(o => o.Items.Select(i => new { Market = o.Farmer.MarketLinks.FirstOrDefault()?.Market.Name ?? "Unassigned", i.LineTotal }))
                .GroupBy(x => x.Market)
                .Select(g => new MarketRevenueViewModel { Market = g.Key, Revenue = g.Sum(x => x.LineTotal) })
                .OrderByDescending(m => m.Revenue)
                .ToList(),
            TopProducts = completed
                .SelectMany(o => o.Items)
                .GroupBy(i => i.ProductName)
                .Select(g => new TopProductViewModel { Name = g.Key, UnitsSold = g.Sum(i => i.Quantity), Revenue = g.Sum(i => i.LineTotal) })
                .OrderByDescending(t => t.Revenue)
                .Take(8)
                .ToList(),
            MostActiveFarmers = completed
                .GroupBy(o => o.Farmer)
                .Select(g => new TopFarmerViewModel
                {
                    FarmerId = g.Key.Id,
                    Name = g.Key.FullName,
                    StallName = g.Key.StallName ?? "",
                    OrderCount = g.Count(),
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(f => f.OrderCount)
                .Take(8)
                .ToList(),
            Orders = orders.OrderByDescending(o => o.OrderDate).Take(25).ToList()
        };

        // Log the generated report
        var admin = await _auth.CurrentUser(User);
        if (admin != null)
        {
            _db.Reports.Add(new Report
            {
                GeneratedBy = admin.Id,
                ReportType = $"Sales Report {start:yyyy-MM-dd} → {vm.To:yyyy-MM-dd}",
                Summary = $"Orders: {vm.TotalOrders} · Revenue: Rs {vm.Revenue:N0} · Markets: {vm.RevenueByMarket.Count}"
            });
            await _db.SaveChangesAsync();
        }

        return View(vm);
    }

    // ───────────────────────── Announcements ─────────────────────────
    public async Task<IActionResult> Announcements()
    {
        var items = await _db.Announcements.OrderByDescending(a => a.CreatedAt).ToListAsync();
        return View(items);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveAnnouncement(int id, string title, string message)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
        {
            TempData["Error"] = "Title and message are required.";
            return RedirectToAction(nameof(Announcements));
        }

        if (id == 0)
        {
            _db.Announcements.Add(new Announcement { Title = title.Trim(), Message = message.Trim() });
            TempData["Success"] = "Announcement published.";
        }
        else
        {
            var item = await _db.Announcements.FindAsync(id);
            if (item != null)
            {
                item.Title = title.Trim();
                item.Message = message.Trim();
                TempData["Success"] = "Announcement updated.";
            }
        }
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Announcements));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleAnnouncement(int id)
    {
        var item = await _db.Announcements.FindAsync(id);
        if (item != null)
        {
            item.IsActive = !item.IsActive;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Announcements));
    }
}
