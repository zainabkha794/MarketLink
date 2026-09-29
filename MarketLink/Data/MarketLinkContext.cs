using MarketLink.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Data;

public class MarketLinkContext : DbContext
{
    public MarketLinkContext(DbContextOptions<MarketLinkContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Market> Markets => Set<Market>();
    public DbSet<MarketFarmer> MarketFarmers => Set<MarketFarmer>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<PickupSlot> PickupSlots => Set<PickupSlot>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasConversion<int>();
            e.Property(u => u.FullName).HasMaxLength(80);
        });

        modelBuilder.Entity<MarketFarmer>(e =>
        {
            e.HasIndex(mf => new { mf.MarketId, mf.FarmerId }).IsUnique();
            e.HasOne(mf => mf.Market).WithMany(m => m.FarmerLinks).HasForeignKey(mf => mf.MarketId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(mf => mf.Farmer).WithMany(u => u.MarketLinks).HasForeignKey(mf => mf.FarmerId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.Price).HasPrecision(10, 2);
            e.HasOne(p => p.Farmer).WithMany(u => u.Products).HasForeignKey(p => p.FarmerId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(p => new { p.Name, p.FarmerId });
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.Property(o => o.TotalAmount).HasPrecision(10, 2);
            e.HasIndex(o => o.OrderNumber).IsUnique();
            e.Property(o => o.Status).HasConversion<int>();
            e.HasOne(o => o.Customer).WithMany(u => u.Orders).HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(o => o.Farmer).WithMany().HasForeignKey(o => o.FarmerId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.Property(i => i.UnitPrice).HasPrecision(10, 2);
            e.Ignore(i => i.LineTotal);
            e.HasOne(i => i.Order).WithMany(o => o.Items).HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(i => i.Product).WithMany(p => p.OrderItems).HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Review>(e =>
        {
            e.HasOne(r => r.Product).WithMany(p => p.Reviews).HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.Customer).WithMany(u => u.Reviews).HasForeignKey(r => r.CustomerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(r => r.Farmer).WithMany().HasForeignKey(r => r.FarmerId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Favorite>(e =>
        {
            // Restrict on Customer/Farmer avoids SQL Server "multiple cascade paths" errors
            e.HasOne(f => f.Customer).WithMany(u => u.Favorites).HasForeignKey(f => f.CustomerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(f => f.Product).WithMany(p => p.Favorites).HasForeignKey(f => f.ProductId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(f => f.Farmer).WithMany().HasForeignKey(f => f.FarmerId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(f => new { f.CustomerId, f.ProductId }).IsUnique().HasFilter("[ProductId] IS NOT NULL");
            e.HasIndex(f => new { f.CustomerId, f.FarmerId }).IsUnique().HasFilter("[FarmerId] IS NOT NULL");
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.HasOne(n => n.User).WithMany(u => u.Notifications).HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PickupSlot>(e =>
        {
            e.HasOne(s => s.Farmer).WithMany(u => u.PickupSlots).HasForeignKey(s => s.FarmerId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Report>(e =>
        {
            e.HasOne(r => r.GeneratedByUser).WithMany().HasForeignKey(r => r.GeneratedBy).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
