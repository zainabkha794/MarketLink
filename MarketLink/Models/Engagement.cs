using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

/// <summary>Customer review of a product (and its farmer).</summary>
public class Review
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public int FarmerId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

    /// <summary>Optional public response from the farmer.</summary>
    [StringLength(600)]
    public string? FarmerResponse { get; set; }
    public DateTime? ResponseDate { get; set; }

    /// <summary>Hidden by admin moderation.</summary>
    public bool IsVisible { get; set; } = true;

    public Product Product { get; set; } = null!;
    public User Customer { get; set; } = null!;
    public User Farmer { get; set; } = null!;
}

/// <summary>Saved farmer or product for quick access + restock alerts.</summary>
public class Favorite
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int? ProductId { get; set; }
    public int? FarmerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Customer { get; set; } = null!;
    public Product? Product { get; set; }
    public User? Farmer { get; set; }
}

/// <summary>In-app alert for order updates, restocks and announcements.</summary>
public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string Message { get; set; } = string.Empty;

    [StringLength(40)]
    public string Type { get; set; } = "Info";

    [StringLength(200)]
    public string? LinkUrl { get; set; }

    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}

/// <summary>Platform-wide announcement published by the admin.</summary>
public class Announcement
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Message { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>Audit log of generated admin reports.</summary>
public class Report
{
    public int Id { get; set; }
    public int GeneratedBy { get; set; }

    [StringLength(80)]
    public string ReportType { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Summary { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public User GeneratedByUser { get; set; } = null!;
}
