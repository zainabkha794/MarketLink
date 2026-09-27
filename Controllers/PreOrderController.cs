using MarketLink.Models;
using MarketLink.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MarketLink.Controllers;

public class PreOrderController : Controller
{
    private readonly IProductService _products;
    private readonly IOrderService _orders;
    private readonly MarketLink.Data.MarketLinkContext _db;

    // Constructor Injection
    public PreOrderController(
        IProductService products,
        IOrderService orders,
        MarketLink.Data.MarketLinkContext db)
    {
        _products = products;
        _orders = orders;
        _db = db;
    }

    // Helper property to retrieve logged-in user ID from Session
    private int? UserId => HttpContext.Session.GetInt32("UserId");

    #region Session Cart Helpers

    // Get cart dictionary (ProductId -> Quantity) from session
    private Dictionary<int, int> GetCart()
    {
        var raw = HttpContext.Session.GetString("Cart");

        if (string.IsNullOrWhiteSpace(raw))
        {
            return new Dictionary<int, int>();
        }

        return JsonSerializer.Deserialize<Dictionary<int, int>>(raw)
               ?? new Dictionary<int, int>();
    }

    // Save cart dictionary back to session
    private void SaveCart(Dictionary<int, int> cart)
    {
        HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
    }

    #endregion

    #region Cart Management

    // GET: /PreOrder/Cart
    // Displays items currently in the user's session cart
    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        var cart = GetCart();

        var products = await _db.Products
            .Where(x => cart.Keys.Contains(x.Id))
            .ToListAsync();

        return View(new CartVm
        {
            Products = products,
            Quantities = cart
        });
    }

    // POST: /PreOrder/AddToCart
    // Adds a product or increments quantity in session cart
    [HttpPost]
    public IActionResult AddToCart(int id, int quantity = 1)
    {
        var cart = GetCart();

        cart[id] = cart.GetValueOrDefault(id) + Math.Max(1, quantity);

        SaveCart(cart);

        return RedirectToAction(nameof(Cart));
    }

    // POST: /PreOrder/RemoveFromCart
    // Removes an item completely from session cart
    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        var cart = GetCart();

        cart.Remove(id);

        SaveCart(cart);

        return RedirectToAction(nameof(Cart));
    }

    #endregion

    #region Checkout Process

    // GET: /PreOrder/Checkout
    // Renders the checkout review page (requires authentication)
    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        if (UserId == null)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/PreOrder/Checkout" });
        }

        var cart = GetCart();

        var products = await _db.Products
            .Where(x => cart.Keys.Contains(x.Id))
            .ToListAsync();

        return View(new CartVm
        {
            Products = products,
            Quantities = cart
        });
    }

    // POST: /PreOrder/Checkout
    // Creates the order from cart items and clears the session cart
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(string address)
    {
        if (UserId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = GetCart();

        try
        {
            var items = cart
                .Select(x => (x.Key, x.Value))
                .ToList();

            var order = await _orders.CreateAsync(
                UserId.Value,
                items,
                address
            );

            // Clear cart after successful order
            SaveCart(new Dictionary<int, int>());

            return RedirectToAction(nameof(Confirmation), new { id = order.Id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction(nameof(Checkout));
        }
    }

    // GET: /PreOrder/Confirmation/{id}
    // Renders order confirmation page with items and product details
    [HttpGet]
    public async Task<IActionResult> Confirmation(int id)
    {
        var order = await _db.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    #endregion
}

// View Model for Cart and Checkout Views
public class CartVm
{
    public List<Product> Products { get; set; } = new();

    public Dictionary<int, int> Quantities { get; set; } = new();

    public decimal Total => Products.Sum(product => product.Price * Quantities.GetValueOrDefault(product.Id));
}