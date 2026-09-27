using MarketLink.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MarketLink.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(MarketLinkContext db)
    {
        if (await db.Users.AnyAsync()) return;

        var admin = new User
        {
            FullName = "System Admin",
            Email = "admin@marketlink.local",
            PasswordHash = Hash("Admin@123"),
            Role = UserRole.Admin
        };

        var farmer = new User
        {
            FullName = "Ali Farmer",
            Email = "farmer@marketlink.local",
            PasswordHash = Hash("Farmer@123"),
            Role = UserRole.Farmer,
            Phone = "03001234567",
            Address = "Karachi"
        };

        var customer = new User
        {
            FullName = "Demo Customer",
            Email = "customer@marketlink.local",
            PasswordHash = Hash("Customer@123"),
            Role = UserRole.Customer,
            Phone = "03007654321",
            Address = "Karachi"
        };

        db.Users.AddRange(admin, farmer, customer);
        await db.SaveChangesAsync();

        db.Markets.Add(new Market
        {
            Name = "Karachi Fresh Market",
            Location = "Karachi",
            Description = "Fresh produce directly from local farmers."
        });

        db.Products.AddRange(
            new Product
            {
                Name = "Fresh Tomatoes",
                Category = "Vegetables",
                Price = 180,
                StockQuantity = 100,
                Unit = "kg",
                FarmerId = farmer.Id,
                Description = "Fresh locally grown tomatoes.",
                ImageUrl = "https://images.unsplash.com/photo-1592924357228-91a4daadcfea?w=600&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Organic Potatoes",
                Category = "Vegetables",
                Price = 140,
                StockQuantity = 150,
                Unit = "kg",
                FarmerId = farmer.Id,
                Description = "Clean, quality potatoes.",
                ImageUrl = "https://images.unsplash.com/photo-1518977676601-b53f82aba655?w=600&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Mangoes",
                Category = "Fruits",
                Price = 350,
                StockQuantity = 80,
                Unit = "kg",
                FarmerId = farmer.Id,
                Description = "Seasonal sweet mangoes.",
                ImageUrl = "https://images.unsplash.com/photo-1553279768-865429fa0078?w=600&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Fresh Milk",
                Category = "Dairy",
                Price = 220,
                StockQuantity = 60,
                Unit = "liter",
                FarmerId = farmer.Id,
                Description = "Fresh farm milk.",
                ImageUrl = "https://images.unsplash.com/photo-1563636619-e9143da7973b?w=600&auto=format&fit=crop&q=80"
            }
        );

        await db.SaveChangesAsync();
    }

    public static string Hash(string value)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }
}