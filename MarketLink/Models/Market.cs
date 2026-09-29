using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

/// <summary>A physical farmers market location shown on the map.</summary>
public class Market
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(800)]
    public string? Description { get; set; }

    [Required, StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [StringLength(80)]
    public string City { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    /// <summary>Google Maps or OpenStreetMap (default).</summary>
    [StringLength(30)]
    public string MapProvider { get; set; } = "OpenStreetMap";

    /// <summary>Comma separated operating days, e.g. "Saturday,Sunday".</summary>
    [StringLength(80)]
    public string OperatingDays { get; set; } = string.Empty;

    public TimeSpan OpenTime { get; set; } = new TimeSpan(8, 0, 0);
    public TimeSpan CloseTime { get; set; } = new TimeSpan(14, 0, 0);

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<MarketFarmer> FarmerLinks { get; set; } = new List<MarketFarmer>();
}

/// <summary>Farmer ↔ Market association (a farmer sells at several markets).</summary>
public class MarketFarmer
{
    public int Id { get; set; }
    public int MarketId { get; set; }
    public int FarmerId { get; set; }

    [StringLength(120)]
    public string? StallLocation { get; set; }

    public Market Market { get; set; } = null!;
    public User Farmer { get; set; } = null!;
}
