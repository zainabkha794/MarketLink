using MarketLink.Data;
using MarketLink.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class HomeController : Controller
{
    private readonly MarketLinkContext _db;

    // Constructor Injection
    public HomeController(MarketLinkContext db)
    {
        _db = db;
    }

    // GET: /Home/Index (Landing Page - Top 8 Latest Active Products)
    public async Task<IActionResult> Index()
    {
        var featuredProducts = await _db.Products
            .Include(x => x.Farmer)
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .Take(8)
            .ToListAsync();

        return View(featuredProducts);
    }

    // GET: /Home/About
    public IActionResult About()
    {
        return View();
    }

    // GET: /Home/Contact
    public IActionResult Contact()
    {
        return View();
    }

    // GET: /Home/Privacy
    public IActionResult Privacy()
    {
        return View();
    }

    // GET: /Home/Error
    public IActionResult Error()
    {
        var errorModel = new ErrorViewModel
        {
            RequestId = HttpContext.TraceIdentifier
        };

        return View(errorModel);
    }
}