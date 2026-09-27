using MarketLink.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Data;

public class MarketLinkContext : DbContext
{
    public MarketLinkContext(DbContextOptions<MarketLinkContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Market> Markets => Set<Market>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        b.Entity<Product>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        b.Entity<Order>()
            .Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        b.Entity<OrderItem>()
            .Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        b.Entity<Product>()
            .HasOne(x => x.Farmer)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.FarmerId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Order>()
            .HasOne(x => x.Customer)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Review>()
            .HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}