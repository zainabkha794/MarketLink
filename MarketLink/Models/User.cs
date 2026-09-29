using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

/// <summary>Platform account: Admin, Farmer or Customer.</summary>
public class User
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Customer;

    [StringLength(30)]
    public string? Phone { get; set; }

    [StringLength(250)]
    public string? Address { get; set; }

    /// <summary>Farmer stall / business name.</summary>
    [StringLength(120)]
    public string? StallName { get; set; }

    /// <summary>Short farmer story shown on the public profile.</summary>
    [StringLength(1000)]
    public string? Bio { get; set; }

    [StringLength(300)]
    public string? ProfileImageUrl { get; set; }

    /// <summary>Stall / pickup pin on the map.</summary>
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    /// <summary>Farmers must be approved by an admin before listing produce.</summary>
    public bool IsApproved { get; set; }

    /// <summary>Accounts can be activated / deactivated (policy violations).</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Hours before pickup when an order can no longer be modified.</summary>
    public int OrderCutoffHours { get; set; } = 12;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<MarketFarmer> MarketLinks { get; set; } = new List<MarketFarmer>();
    public ICollection<PickupSlot> PickupSlots { get; set; } = new List<PickupSlot>();
}
