using MarketLink.Data;
using MarketLink.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class CustomerController : Controller
{
    private readonly MarketLinkContext _db;
    private readonly IOrderService _orders;

    public CustomerController(MarketLinkContext db, IOrderService orders)
    {
        _db = db;
        _orders = orders;
    }

    public async Task<IActionResult> Dashboard()
    {
        var id = HttpContext.Session.GetInt32("UserId");

        if (id == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(
            await _orders.ForCustomerAsync(id.Value)
        );
    }

    // Product View Details Action
    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products
            .Include(p => p.Farmer)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
}