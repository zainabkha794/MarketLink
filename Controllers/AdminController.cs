using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class AdminController : Controller
{
    private readonly MarketLinkContext _db;
    private readonly IOrderService _orders;

    public AdminController(
        MarketLinkContext db,
        IOrderService orders)
    {
        _db = db;
        _orders = orders;
    }

    public async Task<IActionResult> Dashboard()
    {
        var role = HttpContext.Session.GetString("Role");

        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        return View(new AdminVm
        {
            Users = await _db.Users
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(),

            Products = await _db.Products
                .Include(x => x.Farmer)
                .ToListAsync(),

            Orders = await _orders.AllAsync()
        });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(
        int id,
        OrderStatus status)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return Forbid();
        }

        await _orders.UpdateStatusAsync(id, status);

        return RedirectToAction("Dashboard");
    }
}

public class AdminVm
{
    public List<User> Users { get; set; } = new();

    public List<Product> Products { get; set; } = new();

    public List<Order> Orders { get; set; } = new();
}

