using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class HomeController : Controller
{
    private readonly MarketLinkContext _db;
    public HomeController(MarketLinkContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var vm = new HomeViewModel
        {
            FeaturedProducts = await _db.Products
                .Include(p => p.Farmer)
                .Include(p => p.Category)
                .Where(p => p.IsVisible && p.IsAvailable)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync(),
            Categories = await _db.Categories.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToListAsync(),
            Markets = await _db.Markets.Where(m => m.IsActive).Take(3).ToListAsync(),
            Announcements = await _db.Announcements.Where(a => a.IsActive).OrderByDescending(a => a.CreatedAt).Take(2).ToListAsync(),
            FarmerCount = await _db.Users.CountAsync(u => u.Role == UserRole.Farmer && u.IsApproved && u.IsActive),
            ProductCount = await _db.Products.CountAsync(p => p.IsVisible && p.IsAvailable),
            MarketCount = await _db.Markets.CountAsync(m => m.IsActive),
            OrderCount = await _db.Orders.CountAsync()
        };
        return View(vm);
    }

    public IActionResult About() => View();

    public async Task<IActionResult> Contact()
    {
        ViewBag.Markets = await _db.Markets.Where(m => m.IsActive).ToListAsync();
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? code)
    {
        var message = code switch
        {
            403 => "You do not have permission to view this page.",
            404 => "The page you were looking for could not be found.",
            _ => "An unexpected error occurred while processing your request."
        };
        return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier, Message = message });
    }
}
