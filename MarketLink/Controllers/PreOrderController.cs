using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Models.ViewModels;
using MarketLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

/// <summary>Cart, checkout and pre-order management for customers.</summary>
public class PreOrderController : Controller
{
    private readonly MarketLinkContext _db;
    private readonly CartService _cart;
    private readonly IAuthService _auth;
    private readonly NotificationService _notify;

    public PreOrderController(MarketLinkContext db, CartService cart, IAuthService auth, NotificationService notify)
    {
        _db = db;
        _cart = cart;
        _auth = auth;
        _notify = notify;
    }

    // ───────────────────────── Cart ─────────────────────────
    public async Task<IActionResult> Cart() => View(await _cart.GetCartAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1, string? returnUrl = null)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null || !product.IsAvailable || product.IsSoldOut)
        {
            TempData["Error"] = "That item is not available right now.";
        }
        else if (quantity < 1 || quantity > Math.Max(product.StockQuantity, 1))
        {
            TempData["Error"] = "Please choose an available quantity.";
        }
        else
        {
            _cart.Add(productId, quantity);
            TempData["Success"] = $"{product.Name} added to your basket.";
        }

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("Cart");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult UpdateCart(int productId, int quantity)
    {
        _cart.Update(productId, quantity);
        return RedirectToAction("Cart");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult RemoveFromCart(int productId)
    {
        _cart.Remove(productId);
        return RedirectToAction("Cart");
    }

    // ───────────────────────── Checkout ─────────────────────────
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Checkout()
    {
        var cart = await _cart.GetCartAsync();
        if (cart.Items.Count == 0)
        {
            TempData["Info"] = "Your basket is empty.";
            return RedirectToAction("Index", "Marketplace");
        }

        var vm = new CheckoutViewModel { Cart = cart };
        await LoadSlotsForCartAsync(cart, vm);
        return View(vm);
    }

    [Authorize(Roles = "Customer"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        var cart = await _cart.GetCartAsync();
        model.Cart = cart;
        if (cart.Items.Count == 0)
        {
            TempData["Info"] = "Your basket is empty.";
            return RedirectToAction("Index", "Marketplace");
        }

        if (model.PickupDate.Date < DateTime.Today)
            ModelState.AddModelError(nameof(model.PickupDate), "Pickup date cannot be in the past.");
        if (string.IsNullOrWhiteSpace(model.PickupWindow))
            ModelState.AddModelError(nameof(model.PickupWindow), "Please choose a pickup window.");

        await LoadSlotsForCartAsync(cart, model);
        if (!ModelState.IsValid) return View(model);

        var customer = await _auth.CurrentUser(User);
        if (customer == null) return RedirectToAction("Login", "Account");

        // One order per farmer — each stall prepares its own basket.
        var groups = cart.Items.GroupBy(i => i.FarmerId).ToList();
        var orders = new List<Order>();

        foreach (var group in groups)
        {
            var farmer = await _db.Users.FindAsync(group.Key);
            if (farmer == null) continue;

            var order = new Order
            {
                OrderNumber = $"ML-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
                CustomerId = customer.Id,
                FarmerId = farmer.Id,
                Status = OrderStatus.Placed,
                PickupDate = model.PickupDate.Date,
                PickupWindow = model.PickupWindow,
                CustomerNote = model.CustomerNote,
                TotalAmount = group.Sum(i => i.LineTotal)
            };

            foreach (var item in group)
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product == null) continue;

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Unit = product.Unit,
                    UnitPrice = product.Price,
                    Quantity = item.Quantity
                });

                // Reserve stock
                product.StockQuantity = Math.Max(0, product.StockQuantity - item.Quantity);
                if (product.StockQuantity == 0) product.IsSoldOut = true;
            }

            _db.Orders.Add(order);
            orders.Add(order);
            await _db.SaveChangesAsync();

            await _notify.NotifyAsync(farmer.Id,
                "New pre-order received",
                $"{customer.FullName} placed pre-order {order.OrderNumber} for pickup on {model.PickupDate:dddd, d MMM}.",
                "Order", "/Farmer/Orders");
        }

        _cart.Clear();
        TempData["Success"] = "Your pre-orders are placed — pay at pickup. See you at the market!";
        return RedirectToAction("Confirmation", new { count = orders.Count });
    }

    public IActionResult Confirmation(int count = 1)
    {
        ViewBag.OrderCount = count;
        return View();
    }

    private async Task LoadSlotsForCartAsync(CartViewModel cart, CheckoutViewModel model)
    {
        var farmerIds = cart.Items.Select(i => i.FarmerId).Distinct().ToList();
        model.PickupSlots = await _db.PickupSlots
            .Where(s => farmerIds.Contains(s.FarmerId) && s.IsActive)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    // ───────────────────────── Orders ─────────────────────────
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Orders()
    {
        var user = await _auth.CurrentUser(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var orders = await _db.Orders
            .Include(o => o.Farmer)
            .Include(o => o.Items)
            .Where(o => o.CustomerId == user.Id)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        return View(orders);
    }

    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> OrderDetails(int id)
    {
        var user = await _auth.CurrentUser(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var order = await _db.Orders
            .Include(o => o.Farmer)
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user.Id);
        if (order == null) return NotFound();

        var canModify = order.Status is OrderStatus.Placed or OrderStatus.Accepted
                        && DateTime.UtcNow < order.PickupDate.AddHours(-order.Farmer.OrderCutoffHours);

        var canReview = order.Status == OrderStatus.Completed;
        return View(new OrderDetailsViewModel { Order = order, CanModify = canModify, CanReview = canReview });
    }

    [Authorize(Roles = "Customer"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var user = await _auth.CurrentUser(User);
        var order = await _db.Orders.Include(o => o.Farmer).Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user!.Id);

        if (order == null) return NotFound();
        if (order.Status is not (OrderStatus.Placed or OrderStatus.Accepted))
        {
            TempData["Error"] = "This order can no longer be cancelled.";
            return RedirectToAction(nameof(OrderDetails), new { id });
        }
        if (DateTime.UtcNow >= order.PickupDate.AddHours(-order.Farmer.OrderCutoffHours))
        {
            TempData["Error"] = $"The farmer's cut-off time has passed ({order.Farmer.OrderCutoffHours}h before pickup).";
            return RedirectToAction(nameof(OrderDetails), new { id });
        }

        order.Status = OrderStatus.Cancelled;
        order.CancelledAt = DateTime.UtcNow;

        // Return reserved stock
        foreach (var item in order.Items)
        {
            var product = await _db.Products.FindAsync(item.ProductId);
            if (product == null) continue;
            product.StockQuantity += item.Quantity;
            if (product.IsSoldOut && product.StockQuantity > 0) product.IsSoldOut = false;
        }
        await _db.SaveChangesAsync();

        await _notify.NotifyAsync(order.FarmerId,
            "Pre-order cancelled",
            $"Order {order.OrderNumber} was cancelled by the customer.",
            "Order", "/Farmer/Orders");

        TempData["Success"] = "Order cancelled — the stock has been returned to the stall.";
        return RedirectToAction(nameof(Orders));
    }

    [Authorize(Roles = "Customer"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderItems(int id, int[] quantities, int[] itemIds)
    {
        var user = await _auth.CurrentUser(User);
        var order = await _db.Orders.Include(o => o.Farmer).Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user!.Id);

        if (order == null) return NotFound();
        if (order.Status is not (OrderStatus.Placed or OrderStatus.Accepted) ||
            DateTime.UtcNow >= order.PickupDate.AddHours(-order.Farmer.OrderCutoffHours))
        {
            TempData["Error"] = "This order can no longer be modified.";
            return RedirectToAction(nameof(OrderDetails), new { id });
        }

        for (var i = 0; i < itemIds.Length && i < quantities.Length; i++)
        {
            var item = order.Items.FirstOrDefault(x => x.Id == itemIds[i]);
            if (item == null) continue;
            var newQty = Math.Max(0, quantities[i]);
            var product = await _db.Products.FindAsync(item.ProductId);

            if (product != null)
            {
                // Adjust stock by the difference
                var diff = newQty - item.Quantity;
                product.StockQuantity = Math.Max(0, product.StockQuantity - diff);
                if (product.StockQuantity == 0) product.IsSoldOut = true;
                else if (product.IsSoldOut && product.StockQuantity > 0) product.IsSoldOut = false;
            }
            item.Quantity = newQty;
        }

        order.Items.Where(i => i.Quantity == 0).ToList().ForEach(i => order.Items.Remove(i));
        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Order updated.";
        return RedirectToAction(nameof(OrderDetails), new { id });
    }

    [Authorize(Roles = "Customer"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Reorder(int id)
    {
        var user = await _auth.CurrentUser(User);
        var order = await _db.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user!.Id);
        if (order == null) return NotFound();

        var added = 0;
        foreach (var item in order.Items)
        {
            var product = item.Product;
            if (product != null && product.IsAvailable && !product.IsSoldOut && product.StockQuantity > 0)
            {
                _cart.Add(product.Id, Math.Min(item.Quantity, product.StockQuantity));
                added++;
            }
        }

        TempData[added > 0 ? "Success" : "Info"] = added > 0
            ? $"{added} item(s) from order {order.OrderNumber} added to your basket."
            : "None of those items are currently available.";
        return RedirectToAction("Cart");
    }
}
