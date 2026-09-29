namespace MarketLink.Models;

/// <summary>User roles with role-based access control.</summary>
public enum UserRole { Admin = 0, Farmer = 1, Customer = 2 }

/// <summary>Pre-order lifecycle: Placed → Accepted → ReadyForPickup → Completed.</summary>
public enum OrderStatus
{
    Placed = 0,
    Accepted = 1,
    Declined = 2,
    ReadyForPickup = 3,
    Completed = 4,
    Cancelled = 5
}

public static class ProductCategories
{
    public const string Vegetables = "Vegetables";
    public const string Fruits = "Fruits";
    public const string Dairy = "Dairy & Eggs";
    public const string Bakery = "Baked Goods";
    public const string Herbs = "Herbs & Greens";
    public const string Honey = "Honey & Preserves";
    public const string Grains = "Grains & Pulses";
    public const string Flowers = "Flowers & Plants";

    public static readonly string[] All =
    {
        Vegetables, Fruits, Dairy, Bakery, Herbs, Honey, Grains, Flowers
    };
}
