using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = "";

    [MaxLength(1000)]
    public string Description { get; set; } = "";

    [Required, MaxLength(60)]
    public string Category { get; set; } = "";

    [Range(0, 10000000)]
    public decimal Price { get; set; }

    [Range(0, 1000000)]
    public int StockQuantity { get; set; }

    [MaxLength(20)]
    public string Unit { get; set; } = "kg";

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int FarmerId { get; set; }

    public User? Farmer { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}