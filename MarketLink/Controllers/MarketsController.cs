using MarketLink.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class MarketsController : Controller
{
    private readonly MarketLinkContext _db;
    public MarketsController(MarketLinkContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var markets = await _db.Markets
            .Where(m => m.IsActive)
            .Include(m => m.FarmerLinks).ThenInclude(mf => mf.Farmer)
            .OrderBy(m => m.Name)
            .ToListAsync();
        return View(markets);
    }

    public async Task<IActionResult> Details(int id)
    {
        var market = await _db.Markets
            .Include(m => m.FarmerLinks).ThenInclude(mf => mf.Farmer).ThenInclude(f => f.Products)
            .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
        if (market == null) return NotFound();
        return View(market);
    }
}
