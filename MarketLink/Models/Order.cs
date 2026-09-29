using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

/// <summary>A pre-order placed by a customer against one farmer's stock.</summary>
public class Order
{
    public int Id { get; set; }

    [Required, StringLength(24)]
    public string OrderNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public int FarmerId { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Placed;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>Day the customer will collect the basket at the market.</summary>
    public DateTime PickupDate { get; set; }

    /// <summary>Selected pickup window, e.g. "09:00 – 11:00".</summary>
    [StringLength(60)]
    public string PickupWindow { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    [StringLength(600)]
    public string? CustomerNote { get; set; }

    [StringLength(600)]
    public string? FarmerNote { get; set; }

    public DateTime? StatusUpdatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    // Navigation
    public User Customer { get; set; } = null!;
    public User Farmer { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

/// <summary>One line of a pre-order.</summary>
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }

    /// <summary>Snapshot of the product name at order time.</summary>
    [Required, StringLength(120)]
    public string ProductName { get; set; } = string.Empty;

    [StringLength(20)]
    public string Unit { get; set; } = "kg";

    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;

    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}

/// <summary>A pickup window a farmer offers on market days.</summary>
public class PickupSlot
{
    public int Id { get; set; }
    public int FarmerId { get; set; }

    [Required, StringLength(60)]
    public string Label { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsActive { get; set; } = true;

    public User Farmer { get; set; } = null!;
}
