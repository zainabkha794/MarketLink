using MarketLink.Data;
using MarketLink.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class MarketplaceController : Controller
{
    private readonly IProductService _products;
    private readonly MarketLinkContext _db;

    // Constructor Injection
    public MarketplaceController(IProductService products, MarketLinkContext db)
    {
        _products = products;
        _db = db;
    }

    // GET: /Marketplace/Index
    // Catalog view with optional search & category filters
    public async Task<IActionResult> Index(string? search, string? category)
    {
        var viewModel = new MarketplaceVm
        {
            Products = await _products.GetAsync(search, category),
            Search = search,
            Category = category,
            Categories = await _db.Products
                .Where(x => !string.IsNullOrEmpty(x.Category))
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync()
        };

        return View(viewModel);
    }

    // GET: /Marketplace/FarmerProfile/{id}
    // Public profile view for a specific farmer and their listed products
    public async Task<IActionResult> FarmerProfile(int id)
    {
        var farmer = await _db.Users
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id && x.Role == Models.UserRole.Farmer);

        if (farmer == null)
        {
            return NotFound();
        }

        return View(farmer);
    }

    // GET: /Marketplace/Details/{id}
    // Single product details page with direct DbContext fallback for complete image & farmer data
    public async Task<IActionResult> Details(int id)
    {
        // Pehle product fetch karenge
        var product = await _products.GetByIdAsync(id);

        // Fallback: Agar Service layer image ya full object skip kar rahi ho, to direct DB Query run hogi
        if (product == null)
        {
            product = await _db.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
}

// View Model for Marketplace Index
public class MarketplaceVm
{
    public List<Models.Product> Products { get; set; } = new();
    public List<string> Categories { get; set; } = new();
    public string? Search { get; set; }
    public string? Category { get; set; }
}