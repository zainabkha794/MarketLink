using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

/// <summary>Master product category maintained by the admin.</summary>
public class Category
{
    public int Id { get; set; }

    [Required, StringLength(60)]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    /// <summary>Image used on the category tile.</summary>
    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

/// <summary>A weekly stock item published by a farmer.</summary>
public class Product
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1500)]
    public string? Description { get; set; }

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    /// <summary>kg, dozen, liter, bunch, jar, loaf…</summary>
    [Required, StringLength(20)]
    public string Unit { get; set; } = "kg";

    [Range(0, 100000)]
    public int StockQuantity { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public int FarmerId { get; set; }
    public int CategoryId { get; set; }

    /// <summary>Hidden when false (sold out / temporarily unavailable).</summary>
    public bool IsAvailable { get; set; } = true;

    public bool IsSoldOut { get; set; }

    /// <summary>Recurring weekly stock template support.</summary>
    public bool IsRecurringWeekly { get; set; }
    public int DefaultWeeklyQuantity { get; set; }

    /// <summary>Hidden by admin moderation.</summary>
    public bool IsVisible { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User Farmer { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
