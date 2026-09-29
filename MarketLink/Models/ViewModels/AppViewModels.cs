using System.ComponentModel.DataAnnotations;
using MarketLink.Models;

namespace MarketLink.Models.ViewModels;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required, StringLength(80)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(30)]
    [Display(Name = "Contact number")]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class RegisterFarmerViewModel : RegisterViewModel
{
    [Required, StringLength(120)]
    [Display(Name = "Stall / business name")]
    public string StallName { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Bio { get; set; }

    [Range(-90, 90)]
    [Display(Name = "Stall latitude")]
    public double Latitude { get; set; } = 24.8607;

    [Range(-180, 180)]
    [Display(Name = "Stall longitude")]
    public double Longitude { get; set; } = 67.0011;

    [Display(Name = "Markets you sell at")]
    public List<int> MarketIds { get; set; } = new();
}

public class ProfileViewModel
{
    [Required, StringLength(80)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(30)]
    public string? Phone { get; set; }

    [StringLength(250)]
    public string? Address { get; set; }

    [StringLength(120)]
    public string? StallName { get; set; }

    [StringLength(1000)]
    public string? Bio { get; set; }

    [StringLength(300)]
    public string? ProfileImageUrl { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [Range(1, 72)]
    [Display(Name = "Order cut-off (hours before pickup)")]
    public int OrderCutoffHours { get; set; } = 12;

    [Display(Name = "Markets you sell at")]
    public List<int> MarketIds { get; set; } = new();
}

public class ProductFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1500)]
    public string? Description { get; set; }

    [Required, Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [Required, StringLength(20)]
    public string Unit { get; set; } = "kg";

    [Required, Range(0, 100000)]
    [Display(Name = "Quantity available")]
    public int StockQuantity { get; set; }

    [StringLength(300)]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Available for pre-order")]
    public bool IsAvailable { get; set; } = true;

    [Display(Name = "Mark as sold out")]
    public bool IsSoldOut { get; set; }

    [Display(Name = "Repeat this stock every week")]
    public bool IsRecurringWeekly { get; set; }

    [Display(Name = "Default weekly quantity")]
    public int DefaultWeeklyQuantity { get; set; }

    public List<Category> Categories { get; set; } = new();
}

public class MarketplaceFilterViewModel
{
    public string? Query { get; set; }
    public string? Category { get; set; }
    public int? MarketId { get; set; }
    public string? Day { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string Sort { get; set; } = "newest";

    public List<Category> Categories { get; set; } = new();
    public List<Market> Markets { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public int TotalCount { get; set; }
}

public class CartItemViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Unit { get; set; } = "kg";
    public int Quantity { get; set; }
    public int FarmerId { get; set; }
    public string FarmerName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int StockQuantity { get; set; }
    public decimal LineTotal => Price * Quantity;
}

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal Subtotal => Items.Sum(i => i.LineTotal);
    public int TotalItems => Items.Sum(i => i.Quantity);
    public int FarmerCount => Items.Select(i => i.FarmerId).Distinct().Count();
}

public class CheckoutViewModel
{
    public CartViewModel Cart { get; set; } = new();

    [Required]
    [Display(Name = "Pickup date")]
    [DataType(DataType.Date)]
    public DateTime PickupDate { get; set; } = DateTime.Today.AddDays(1);

    [Required]
    [Display(Name = "Pickup window")]
    public string PickupWindow { get; set; } = string.Empty;

    [StringLength(600)]
    [Display(Name = "Note for the farmer (optional)")]
    public string? CustomerNote { get; set; }

    public List<PickupSlot> PickupSlots { get; set; } = new();
    public List<Market> Markets { get; set; } = new();
}

public class HomeViewModel
{
    public List<Product> FeaturedProducts { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Market> Markets { get; set; } = new();
    public List<Announcement> Announcements { get; set; } = new();
    public int FarmerCount { get; set; }
    public int ProductCount { get; set; }
    public int MarketCount { get; set; }
    public int OrderCount { get; set; }
}

public class OrderDetailsViewModel
{
    public Order Order { get; set; } = null!;
    public bool CanModify { get; set; }
    public bool CanReview { get; set; }
}

public class ReviewFormViewModel
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    [StringLength(1000)]
    public string? Comment { get; set; }
}

public class FarmerDashboardViewModel
{
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public decimal Revenue { get; set; }
    public List<Order> RecentOrders { get; set; } = new();
    public List<TopProductViewModel> TopProducts { get; set; } = new();
    public int ProductCount { get; set; }
    public int LowStockCount { get; set; }
    public double AverageRating { get; set; }
    public List<decimal> RevenueByDay { get; set; } = new();
}

public class TopProductViewModel
{
    public string Name { get; set; } = string.Empty;
    public int UnitsSold { get; set; }
    public decimal Revenue { get; set; }
}

public class AdminDashboardViewModel
{
    public int TotalFarmers { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalMarkets { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingFarmerApprovals { get; set; }
    public List<Order> RecentOrders { get; set; } = new();
    public List<TopProductViewModel> TopProducts { get; set; } = new();
    public List<MarketRevenueViewModel> RevenueByMarket { get; set; } = new();
}

public class MarketRevenueViewModel
{
    public string Market { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int Orders { get; set; }
}

public class AdminReportViewModel
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int TotalOrders { get; set; }
    public decimal Revenue { get; set; }
    public List<MarketRevenueViewModel> RevenueByMarket { get; set; } = new();
    public List<TopProductViewModel> TopProducts { get; set; } = new();
    public List<TopFarmerViewModel> MostActiveFarmers { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}

public class TopFarmerViewModel
{
    public int FarmerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StallName { get; set; } = string.Empty;
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
}

public class ChatViewModel
{
    public List<ChatMessageViewModel> Messages { get; set; } = new();
}

public class ChatMessageViewModel
{
    public string Role { get; set; } = "bot"; // bot | user
    public string Text { get; set; } = string.Empty;
    public DateTime Time { get; set; } = DateTime.UtcNow;
}
