using MarketLink.Data;
using MarketLink.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class MarketplaceController : Controller
{
    private readonly MarketLinkContext _db;
    public MarketplaceController(MarketLinkContext db) => _db = db;

    public async Task<IActionResult> Index(MarketplaceFilterViewModel filter)
    {
        var query = _db.Products
            .Include(p => p.Farmer)
            .Include(p => p.Category)
            .Include(p => p.Reviews.Where(r => r.IsVisible))
            .Where(p => p.IsVisible && p.Farmer.IsActive && p.Farmer.IsApproved);

        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            var q = filter.Query.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(q) ||
                (p.Description != null && p.Description.ToLower().Contains(q)) ||
                p.Farmer.FullName.ToLower().Contains(q) ||
                (p.Farmer.StallName != null && p.Farmer.StallName.ToLower().Contains(q)));
        }
        if (!string.IsNullOrWhiteSpace(filter.Category))
            query = query.Where(p => p.Category.Name == filter.Category);
        if (filter.MarketId.HasValue)
            query = query.Where(p => p.Farmer.MarketLinks.Any(mf => mf.MarketId == filter.MarketId));
        if (!string.IsNullOrWhiteSpace(filter.Day))
            query = query.Where(p => p.Farmer.MarketLinks.Any(mf => mf.Market.OperatingDays.Contains(filter.Day!)));
        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice);
        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice);

        query = filter.Sort switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name" => query.OrderBy(p => p.Name),
            "rating" => query.OrderByDescending(p => p.Reviews.Average(r => (double?)r.Rating) ?? 0),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        filter.TotalCount = await query.CountAsync();
        filter.Products = await query.Take(60).ToListAsync();
        filter.Categories = await _db.Categories.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToListAsync();
        filter.Markets = await _db.Markets.Where(m => m.IsActive).ToListAsync();
        return View(filter);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products
            .Include(p => p.Farmer).ThenInclude(f => f.MarketLinks).ThenInclude(mf => mf.Market)
            .Include(p => p.Category)
            .Include(p => p.Reviews.Where(r => r.IsVisible)).ThenInclude(r => r.Customer)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsVisible);

        if (product == null) return NotFound();

        var related = await _db.Products
            .Include(p => p.Farmer)
            .Where(p => p.CategoryId == product.CategoryId && p.Id != id && p.IsVisible && p.IsAvailable)
            .Take(4)
            .ToListAsync();
        ViewBag.Related = related;
        return View(product);
    }

    public async Task<IActionResult> FarmerProfile(int id)
    {
        var farmer = await _db.Users
            .Include(f => f.MarketLinks).ThenInclude(mf => mf.Market)
            .Include(f => f.PickupSlots)
            .FirstOrDefaultAsync(f => f.Id == id && f.Role == Models.UserRole.Farmer && f.IsApproved && f.IsActive);

        if (farmer == null) return NotFound();

        ViewBag.Products = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.FarmerId == id && p.IsVisible && p.IsAvailable)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        ViewBag.Reviews = await _db.Reviews
            .Include(r => r.Customer)
            .Include(r => r.Product)
            .Where(r => r.FarmerId == id && r.IsVisible)
            .OrderByDescending(r => r.ReviewDate)
            .Take(12)
            .ToListAsync();

        ViewBag.AverageRating = await _db.Reviews
            .Where(r => r.FarmerId == id && r.IsVisible)
            .AverageAsync(r => (double?)r.Rating) ?? 0;

        return View(farmer);
    }
}
