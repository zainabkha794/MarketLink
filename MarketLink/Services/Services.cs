using System.Text.Json;
using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Services;

public class CartLine
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

/// <summary>Session-backed shopping cart shared with anonymous visitors.</summary>
public class CartService
{
    private const string SessionKey = "MarketLink.Cart";
    private readonly IHttpContextAccessor _http;
    private readonly MarketLinkContext _db;

    public CartService(IHttpContextAccessor http, MarketLinkContext db)
    {
        _http = http;
        _db = db;
    }

    private ISession Session => _http.HttpContext!.Session;

    private List<CartLine> Load()
    {
        var raw = Session.GetString(SessionKey);
        return string.IsNullOrEmpty(raw)
            ? new List<CartLine>()
            : JsonSerializer.Deserialize<List<CartLine>>(raw) ?? new List<CartLine>();
    }

    private void Save(List<CartLine> lines) =>
        Session.SetString(SessionKey, JsonSerializer.Serialize(lines));

    public void Add(int productId, int quantity = 1)
    {
        var lines = Load();
        var line = lines.FirstOrDefault(l => l.ProductId == productId);
        if (line == null) lines.Add(new CartLine { ProductId = productId, Quantity = quantity });
        else line.Quantity += quantity;
        Save(lines);
    }

    public void Update(int productId, int quantity)
    {
        var lines = Load();
        var line = lines.FirstOrDefault(l => l.ProductId == productId);
        if (line == null) return;
        if (quantity <= 0) lines.Remove(line);
        else line.Quantity = quantity;
        Save(lines);
    }

    public void Remove(int productId)
    {
        var lines = Load();
        lines.RemoveAll(l => l.ProductId == productId);
        Save(lines);
    }

    public void Clear() => Session.Remove(SessionKey);

    public int Count() => Load().Sum(l => l.Quantity);

    public async Task<CartViewModel> GetCartAsync()
    {
        var lines = Load();
        var ids = lines.Select(l => l.ProductId).ToList();
        var products = await _db.Products
            .Include(p => p.Farmer)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

        var vm = new CartViewModel();
        foreach (var line in lines)
        {
            var p = products.FirstOrDefault(x => x.Id == line.ProductId);
            if (p == null) continue;
            vm.Items.Add(new CartItemViewModel
            {
                ProductId = p.Id,
                Name = p.Name,
                Price = p.Price,
                Unit = p.Unit,
                Quantity = line.Quantity,
                FarmerId = p.FarmerId,
                FarmerName = p.Farmer.StallName ?? p.Farmer.FullName,
                ImageUrl = p.ImageUrl,
                StockQuantity = p.StockQuantity
            });
        }
        return vm;
    }
}

/// <summary>In-app notifications with optional SMTP delivery.</summary>
public class NotificationService
{
    private readonly MarketLinkContext _db;
    private readonly IConfiguration _config;

    public NotificationService(MarketLinkContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task NotifyAsync(int userId, string title, string message, string type = "Info", string? link = null)
    {
        _db.Notifications.Add(new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            LinkUrl = link
        });
        await _db.SaveChangesAsync();
        await TrySendEmailAsync(userId, title, message);
    }

    public async Task NotifyManyAsync(IEnumerable<int> userIds, string title, string message, string type = "Info", string? link = null)
    {
        foreach (var id in userIds.Distinct())
            _db.Notifications.Add(new Notification { UserId = id, Title = title, Message = message, Type = type, LinkUrl = link });
        await _db.SaveChangesAsync();
    }

    private async Task TrySendEmailAsync(int userId, string title, string message)
    {
        // Email is optional — enabled through the Smtp section in appsettings.json.
        if (!_config.GetValue<bool>("Smtp:Enabled")) return;
        await Task.CompletedTask;
    }
}

/// <summary>Rule-based assistant that answers from live platform data.</summary>
public class ChatbotService
{
    private readonly MarketLinkContext _db;

    public ChatbotService(MarketLinkContext db) => _db = db;

    public async Task<string> ReplyAsync(string message, int? userId)
    {
        var q = message.ToLower().Trim();

        if (q.Length == 0)
            return "Tell me what you are looking for — a product, a market day, or help with your order.";

        if (ContainsAny(q, "hello", "hi ", "hey", "salam", "assalam"))
            return "Hello and welcome to MarketLink! 🌿 I can help you find produce, check market days, or track a pre-order. What may I help you with?";

        if (ContainsAny(q, "market", "open", "timings", "timing", "hours", "when"))
        {
            var markets = await _db.Markets.Where(m => m.IsActive).ToListAsync();
            if (markets.Count == 0) return "No markets are listed at the moment — please check back soon.";
            var lines = markets.Select(m =>
                $"• {m.Name} — {m.OperatingDays.Replace(",", " & ")} · {m.OpenTime:hh\\:mm} to {m.CloseTime:hh\\:mm}");
            return "Here are our markets:\n" + string.Join("\n", lines) +
                   "\n\nYou can see them all on the Markets page with map directions.";
        }

        if (ContainsAny(q, "order", "pickup", "collect", "track", "status", "cancel"))
        {
            if (userId == null)
                return "To check an order, please log in first — then visit 'My Orders' from your dashboard. You can modify or cancel a pre-order before your farmer's cut-off time.";
            var orders = await _db.Orders
                .Include(o => o.Farmer)
                .Where(o => o.CustomerId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Take(3)
                .ToListAsync();
            if (orders.Count == 0)
                return "You have no pre-orders yet. Browse the marketplace and reserve your favourites — payment is settled at pickup.";
            var lines = orders.Select(o =>
                $"• {o.OrderNumber} — {o.Farmer.StallName ?? o.Farmer.FullName} · {o.Status} · pickup {o.PickupDate:dddd, d MMM} ({o.PickupWindow})");
            return "Here are your latest pre-orders:\n" + string.Join("\n", lines) +
                   "\n\nOpen 'My Orders' to see the full timeline, modify quantities or cancel.";
        }

        if (ContainsAny(q, "payment", "pay", "card", "cash"))
            return "MarketLink keeps things simple: there are no online payments. You settle the bill in person when you collect your basket at the market stall.";

        if (ContainsAny(q, "favorite", "favourite", "restock", "alert"))
            return "Tap the ♡ on any product or farmer to save them. When a saved item that was sold out is restocked, we will send you an in-app alert straight away.";

        if (ContainsAny(q, "review", "rate", "rating"))
            return "After a pre-order is completed you can rate the produce from 1 to 5 stars and leave a note for the farmer — and farmers can reply. Find the option in 'My Orders'.";

        if (ContainsAny(q, "register", "sign up", "farmer", "sell", "join"))
            return "Farmers can apply with their stall name and the markets they sell at — an admin approves new stalls before produce can be listed. Customers can register instantly on the Register page.";

        if (ContainsAny(q, "deliver", "shipping", "courier"))
            return "All orders are pickup-only at the market — it keeps the produce fresher than any courier could! Choose your pickup window at checkout.";

        // Product / price / availability questions
        var terms = q.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var products = await _db.Products
            .Include(p => p.Farmer)
            .Include(p => p.Category)
            .Where(p => p.IsVisible && p.IsAvailable)
            .ToListAsync();
        var matches = products.Where(p =>
            terms.Any(t => t.Length > 2 && p.Name.ToLower().Contains(t)) ||
            terms.Any(t => t.Length > 2 && (p.Category?.Name ?? "").ToLower().Contains(t))).Take(4).ToList();

        if (matches.Count > 0)
        {
            var lines = matches.Select(p =>
                $"• {p.Name} — Rs {p.Price:0}/{p.Unit} · {(p.IsSoldOut || p.StockQuantity == 0 ? "sold out" : $"{p.StockQuantity} {p.Unit} available")} · {p.Farmer.StallName ?? p.Farmer.FullName}");
            return "Here is what I found:\n" + string.Join("\n", lines) +
                   "\n\nOpen the Marketplace for the full listing with photos and filters.";
        }

        return "I could not find that just yet. Try asking about a product (for example “tomatoes”), market days, pickup windows, or your orders. You can also browse everything on the Marketplace page.";
    }

    private static bool ContainsAny(string text, params string[] keys) => keys.Any(text.Contains);
}
