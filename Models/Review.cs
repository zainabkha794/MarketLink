using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

public class Review
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public int CustomerId { get; set; }

    public User? Customer { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(600)]
    public string Comment { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}