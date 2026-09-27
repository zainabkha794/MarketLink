using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

public enum UserRole
{
    Customer,
    Farmer,
    Admin
}

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string FullName { get; set; } = "";

    [Required, MaxLength(120)]
    public string Email { get; set; } = "";

    [Required]
    public string PasswordHash { get; set; } = "";

    [Required]
    public UserRole Role { get; set; } = UserRole.Customer;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}