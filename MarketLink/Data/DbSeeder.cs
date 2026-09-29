using MarketLink.Models;
using MarketLink.Services;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(MarketLinkContext db)
    {
        if (await db.Users.AnyAsync()) return;

        // ───────────────────────── Users ─────────────────────────
        var admin = new User
        {
            FullName = "Eleanor Hartwell",
            Email = "admin@marketlink.local",
            PasswordHash = PasswordHasher.Hash("Admin@123"),
            Role = UserRole.Admin,
            Phone = "+92 21 111 000 111",
            Address = "MarketLink House, Clifton, Karachi",
            IsApproved = true
        };

        var farmers = new[]
        {
            new User
            {
                FullName = "Harold Green",
                Email = "farmer@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Farmer@123"),
                Role = UserRole.Farmer,
                Phone = "+92 300 222 3344",
                Address = "Willowbrook Lane, Malir, Karachi",
                StallName = "Willowbrook Farm",
                Bio = "Four generations of market gardeners. Willowbrook is known for heritage tomatoes, crisp greens and the sweetest strawberries of the season — grown slowly, picked ripe, carried to market the same morning.",
                Latitude = 24.8923, Longitude = 67.0822,
                IsApproved = true, OrderCutoffHours = 12
            },
            new User
            {
                FullName = "Beatrice Ashford",
                Email = "beatrice@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Farmer@123"),
                Role = UserRole.Farmer,
                Phone = "+92 301 555 4488",
                Address = "Stonegate Orchard, Gadap, Karachi",
                StallName = "Stonegate Orchard",
                Bio = "A quiet orchard of mango, guava and citrus trees. We press our own juices and bottle orchard honey each autumn.",
                Latitude = 25.0211, Longitude = 67.0931,
                IsApproved = true, OrderCutoffHours = 18
            },
            new User
            {
                FullName = "James Holloway",
                Email = "james@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Farmer@123"),
                Role = UserRole.Farmer,
                Phone = "+92 333 777 2211",
                Address = "Meadowcroft Dairy, Super Highway, Karachi",
                StallName = "Meadowcroft Dairy",
                Bio = "Pasture-fed cows, glass bottles and butter churned by hand. Our cream-top milk barely makes it past noon on market days.",
                Latitude = 24.9612, Longitude = 67.1204,
                IsApproved = true, OrderCutoffHours = 10
            },
            new User
            {
                FullName = "Adelaide Thornbury",
                Email = "adelaide@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Farmer@123"),
                Role = UserRole.Farmer,
                Phone = "+92 345 888 6677",
                Address = "Thornbury Bakehouse, Zamzama, Karachi",
                StallName = "Thornbury Bakehouse",
                Bio = "Slow-fermented sourdough, honey oat loaves and butter pastries baked before dawn in a stone hearth oven.",
                Latitude = 24.8134, Longitude = 67.0301,
                IsApproved = true, OrderCutoffHours = 24
            },
            new User
            {
                FullName = "Oscar Whitfield",
                Email = "oscar@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Farmer@123"),
                Role = UserRole.Farmer,
                Phone = "+92 302 444 9900",
                Address = "Honeyfield Apiary, Hub River Road, Karachi",
                StallName = "Honeyfield Apiary & Preserves",
                Bio = "Wildflower honey from hives kept at the edge of the countryside, plus small-batch jams and pickles.",
                Latitude = 24.8745, Longitude = 66.9822,
                IsApproved = true, OrderCutoffHours = 12
            },
            new User
            {
                FullName = "Clara Bennett",
                Email = "clara@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Farmer@123"),
                Role = UserRole.Farmer,
                Phone = "+92 311 222 3311",
                Address = "Green Acres Estate, North Nazimabad, Karachi",
                StallName = "Green Acres Estate",
                Bio = "Herbs, salad leaves and edible flowers raised in raised beds under morning light.",
                Latitude = 24.9302, Longitude = 67.0402,
                IsApproved = false, OrderCutoffHours = 12 // awaiting admin approval — demonstrates the approval flow
            }
        };

        var customers = new[]
        {
            new User
            {
                FullName = "Sophia Whitmore",
                Email = "customer@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Customer@123"),
                Role = UserRole.Customer,
                Phone = "+92 321 123 4567",
                Address = "12 Sea View Apartments, Clifton, Karachi"
            },
            new User
            {
                FullName = "Daniel Reyes",
                Email = "daniel@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Customer@123"),
                Role = UserRole.Customer,
                Phone = "+92 333 987 6543",
                Address = "45 Palm Avenue, DHA Phase 6, Karachi"
            },
            new User
            {
                FullName = "Amelia Stone",
                Email = "amelia@marketlink.local",
                PasswordHash = PasswordHasher.Hash("Customer@123"),
                Role = UserRole.Customer,
                Phone = "+92 300 456 7890",
                Address = "8 Garden Terrace, Bath Island, Karachi"
            }
        };

        db.Users.AddRange(admin);
        db.Users.AddRange(farmers);
        db.Users.AddRange(customers);
        await db.SaveChangesAsync();

        var willowbrook = farmers[0];
        var stonegate = farmers[1];
        var meadowcroft = farmers[2];
        var thornbury = farmers[3];
        var honeyfield = farmers[4];

        // ───────────────────────── Markets ─────────────────────────
        var markets = new[]
        {
            new Market
            {
                Name = "Clifton Green Farmers Market",
                Description = "The grand old Saturday market beneath the rain trees — heritage produce, cut flowers and hearth breads.",
                Address = "Clifton Park, Block 5, Clifton", City = "Karachi",
                Latitude = 24.8153, Longitude = 67.0281,
                OperatingDays = "Saturday,Sunday", OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(14, 0, 0),
                ImageUrl = "images/market-clifton.svg"
            },
            new Market
            {
                Name = "Old Town Harvest Market",
                Description = "A weekday market in the old quarter, famous for fruit, dairy and preserves from the outlying estates.",
                Address = "M.A. Jinnah Road, Saddar", City = "Karachi",
                Latitude = 24.8612, Longitude = 67.0098,
                OperatingDays = "Wednesday,Friday", OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(15, 0, 0),
                ImageUrl = "images/market-oldtown.svg"
            },
            new Market
            {
                Name = "Riverside Artisan Market",
                Description = "A Sunday morning gathering by the river — organic greens, small-batch baked goods and live acoustic music.",
                Address = "Riverside Promenade, Korangi Creek", City = "Karachi",
                Latitude = 24.8231, Longitude = 67.1157,
                OperatingDays = "Sunday", OpenTime = new TimeSpan(7, 30, 0), CloseTime = new TimeSpan(12, 30, 0),
                ImageUrl = "images/market-riverside.svg"
            }
        };
        db.Markets.AddRange(markets);
        await db.SaveChangesAsync();

        db.MarketFarmers.AddRange(
            new MarketFarmer { MarketId = markets[0].Id, FarmerId = willowbrook.Id, StallLocation = "Row A · Stall 3" },
            new MarketFarmer { MarketId = markets[0].Id, FarmerId = thornbury.Id, StallLocation = "Row B · Stall 1" },
            new MarketFarmer { MarketId = markets[0].Id, FarmerId = meadowcroft.Id, StallLocation = "Row A · Stall 7" },
            new MarketFarmer { MarketId = markets[1].Id, FarmerId = stonegate.Id, StallLocation = "Row C · Stall 2" },
            new MarketFarmer { MarketId = markets[1].Id, FarmerId = honeyfield.Id, StallLocation = "Row C · Stall 5" },
            new MarketFarmer { MarketId = markets[2].Id, FarmerId = willowbrook.Id, StallLocation = "Row A · Stall 1" },
            new MarketFarmer { MarketId = markets[2].Id, FarmerId = thornbury.Id, StallLocation = "Row B · Stall 4" }
        );

        // ───────────────────────── Categories ─────────────────────────
        var categories = new[]
        {
            new Category { Name = ProductCategories.Vegetables, Description = "Harvested this morning", SortOrder = 1, ImageUrl = "images/cat-vegetables.jpg" },
            new Category { Name = ProductCategories.Fruits, Description = "Tree-ripened & seasonal", SortOrder = 2, ImageUrl = "images/cat-fruits.jpg" },
            new Category { Name = ProductCategories.Dairy, Description = "Milk, butter & eggs", SortOrder = 3, ImageUrl = "images/cat-dairy.jpg" },
            new Category { Name = ProductCategories.Bakery, Description = "Stone-hearth baked", SortOrder = 4, ImageUrl = "images/cat-bakery.jpg" },
            new Category { Name = ProductCategories.Herbs, Description = "Aromatic & fresh", SortOrder = 5, ImageUrl = "images/cat-herbs.jpg" },
            new Category { Name = ProductCategories.Honey, Description = "Golden & preserved", SortOrder = 6, ImageUrl = "images/cat-honey.jpg" },
            new Category { Name = ProductCategories.Grains, Description = "Stone-milled staples", SortOrder = 7, ImageUrl = "images/cat-grains.jpg" },
            new Category { Name = ProductCategories.Flowers, Description = "Cut & potted", SortOrder = 8, ImageUrl = "images/cat-flowers.jpg" }
        };
        db.Categories.AddRange(categories);
        await db.SaveChangesAsync();

        Category Cat(string name) => categories.First(c => c.Name == name);

        // ───────────────────────── Products ─────────────────────────
        var products = new[]
        {
            new Product
            {
                Name = "Heritage Vine Tomatoes", CategoryId = Cat(ProductCategories.Vegetables).Id,
                Description = "Deep-ribbed heirloom tomatoes ripened on the vine. Rich, sweet and perfect for a summer salad.",
                Price = 240, Unit = "kg", StockQuantity = 60, FarmerId = willowbrook.Id,
                ImageUrl = "https://images.unsplash.com/photo-1592924357228-91a4daadcfea?w=800&auto=format&fit=crop&q=80",
                IsRecurringWeekly = true, DefaultWeeklyQuantity = 60
            },
            new Product
            {
                Name = "Morning Leaf Spinach", CategoryId = Cat(ProductCategories.Vegetables).Id,
                Description = "Tender young spinach picked at first light, still cool from the field.",
                Price = 120, Unit = "bunch", StockQuantity = 45, FarmerId = willowbrook.Id,
                ImageUrl = "https://images.unsplash.com/photo-1576045057995-568f588f82fb?w=800&auto=format&fit=crop&q=80",
                IsRecurringWeekly = true, DefaultWeeklyQuantity = 45
            },
            new Product
            {
                Name = "Golden Baby Potatoes", CategoryId = Cat(ProductCategories.Vegetables).Id,
                Description = "Buttery yellow potatoes, dug the evening before market day.",
                Price = 160, Unit = "kg", StockQuantity = 80, FarmerId = willowbrook.Id,
                ImageUrl = "https://images.unsplash.com/photo-1518977676601-b53f82aba655?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Sweet Carrots (Bunched)", CategoryId = Cat(ProductCategories.Vegetables).Id,
                Description = "Crunchy, sweet carrots with their tops on — evidence of true freshness.",
                Price = 140, Unit = "bunch", StockQuantity = 50, FarmerId = willowbrook.Id,
                ImageUrl = "https://images.unsplash.com/photo-1445282768818-728615cc910a?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Cherry Tomato Medley", CategoryId = Cat(ProductCategories.Vegetables).Id,
                Description = "Ruby, amber and chocolate cherry tomatoes in a paper pint box.",
                Price = 300, Unit = "box", StockQuantity = 30, FarmerId = willowbrook.Id,
                ImageUrl = "https://images.unsplash.com/photo-1546470427-e26264be0b0d?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Alphonso Mangoes", CategoryId = Cat(ProductCategories.Fruits).Id,
                Description = "The celebrated Alphonso — honeyed, fragrant and utterly indulgent. Limited harvest.",
                Price = 650, Unit = "dozen", StockQuantity = 24, FarmerId = stonegate.Id,
                ImageUrl = "https://images.unsplash.com/photo-1553279768-865429fa0078?w=800&auto=format&fit=crop&q=80",
                IsRecurringWeekly = true, DefaultWeeklyQuantity = 24
            },
            new Product
            {
                Name = "Orchard Guavas", CategoryId = Cat(ProductCategories.Fruits).Id,
                Description = "Fragrant pink-fleshed guavas, hand-picked from Stonegate's oldest trees.",
                Price = 220, Unit = "kg", StockQuantity = 40, FarmerId = stonegate.Id,
                ImageUrl = "https://images.unsplash.com/photo-1536511132770-e5058c7e8c46?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Valencia Oranges", CategoryId = Cat(ProductCategories.Fruits).Id,
                Description = "Juicy, thin-skinned oranges — half a glass of sunshine per fruit.",
                Price = 180, Unit = "kg", StockQuantity = 55, FarmerId = stonegate.Id,
                ImageUrl = "https://images.unsplash.com/photo-1547514701-42782101795e?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Cream-Top Whole Milk", CategoryId = Cat(ProductCategories.Dairy).Id,
                Description = "Non-homogenised whole milk in a glass bottle, with a ribbon of cream on top.",
                Price = 260, Unit = "liter", StockQuantity = 35, FarmerId = meadowcroft.Id,
                ImageUrl = "https://images.unsplash.com/photo-1563636619-e9143da7973b?w=800&auto=format&fit=crop&q=80",
                IsRecurringWeekly = true, DefaultWeeklyQuantity = 35
            },
            new Product
            {
                Name = "Hand-Churned Farm Butter", CategoryId = Cat(ProductCategories.Dairy).Id,
                Description = "Cultured butter churned in small batches, flecked with sea salt.",
                Price = 420, Unit = "block", StockQuantity = 20, FarmerId = meadowcroft.Id,
                ImageUrl = "https://images.unsplash.com/photo-1589985270826-4b7bb135bc9d?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Free-Range Eggs", CategoryId = Cat(ProductCategories.Dairy).Id,
                Description = "A dozen eggs from hens that wander the meadow all day.",
                Price = 320, Unit = "dozen", StockQuantity = 42, FarmerId = meadowcroft.Id,
                ImageUrl = "https://images.unsplash.com/photo-1582722872445-44dc5f7e3c8f?w=800&auto=format&fit=crop&q=80",
                IsRecurringWeekly = true, DefaultWeeklyQuantity = 42
            },
            new Product
            {
                Name = "Meadowcroft Yogurt", CategoryId = Cat(ProductCategories.Dairy).Id,
                Description = "Thick, tangy set yogurt made the traditional way in clay-lined pots.",
                Price = 280, Unit = "jar", StockQuantity = 18, FarmerId = meadowcroft.Id,
                ImageUrl = "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Stone-Hearth Sourdough", CategoryId = Cat(ProductCategories.Bakery).Id,
                Description = "A 36-hour fermented loaf with a burnished crust and open, creamy crumb.",
                Price = 350, Unit = "loaf", StockQuantity = 25, FarmerId = thornbury.Id,
                ImageUrl = "https://images.unsplash.com/photo-1509440159596-0249088772ff?w=800&auto=format&fit=crop&q=80",
                IsRecurringWeekly = true, DefaultWeeklyQuantity = 25
            },
            new Product
            {
                Name = "Honey Oat Loaf", CategoryId = Cat(ProductCategories.Bakery).Id,
                Description = "Soft, lightly sweetened with Honeyfield wildflower honey and rolled oats.",
                Price = 280, Unit = "loaf", StockQuantity = 20, FarmerId = thornbury.Id,
                ImageUrl = "https://images.unsplash.com/photo-1586444248902-2f64eddc13df?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Butter Croissants", CategoryId = Cat(ProductCategories.Bakery).Id,
                Description = "Laminated by hand over two days. Shatteringly flaky, deeply buttery.",
                Price = 180, Unit = "piece", StockQuantity = 36, FarmerId = thornbury.Id,
                ImageUrl = "https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Cinnamon Morning Buns", CategoryId = Cat(ProductCategories.Bakery).Id,
                Description = "Brioche swirls with Ceylon cinnamon and a whisper of orange zest.",
                Price = 160, Unit = "piece", StockQuantity = 0, FarmerId = thornbury.Id,
                ImageUrl = "https://images.unsplash.com/photo-1509365465985-25d11c17e812?w=800&auto=format&fit=crop&q=80",
                IsSoldOut = true
            },
            new Product
            {
                Name = "Garden Herb Bundle", CategoryId = Cat(ProductCategories.Herbs).Id,
                Description = "Basil, mint, coriander and dill tied with garden twine.",
                Price = 150, Unit = "bundle", StockQuantity = 40, FarmerId = willowbrook.Id,
                ImageUrl = "https://images.unsplash.com/photo-1466637574441-749b8f19452f?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Wildflower Honey", CategoryId = Cat(ProductCategories.Honey).Id,
                Description = "Raw, unfiltered honey drawn from hives at the edge of the countryside.",
                Price = 780, Unit = "jar", StockQuantity = 30, FarmerId = honeyfield.Id,
                ImageUrl = "https://images.unsplash.com/photo-1587049352846-4a222e784d38?w=800&auto=format&fit=crop&q=80",
                IsRecurringWeekly = true, DefaultWeeklyQuantity = 30
            },
            new Product
            {
                Name = "Golden Quince Preserve", CategoryId = Cat(ProductCategories.Honey).Id,
                Description = "Slow-cooked quince set to a jewel-like jelly. Wonderful with buttered sourdough.",
                Price = 520, Unit = "jar", StockQuantity = 22, FarmerId = honeyfield.Id,
                ImageUrl = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Stone-Milled Whole Wheat", CategoryId = Cat(ProductCategories.Grains).Id,
                Description = "Nutty, nutrient-rich flour milled slowly between stone wheels.",
                Price = 240, Unit = "kg", StockQuantity = 48, FarmerId = stonegate.Id,
                ImageUrl = "https://images.unsplash.com/photo-1574323347407-f5e1ad6d020b?w=800&auto=format&fit=crop&q=80"
            },
            new Product
            {
                Name = "Fresh Basil Pot", CategoryId = Cat(ProductCategories.Flowers).Id,
                Description = "A living basil plant in a terracotta pot — snip leaves as you need them.",
                Price = 380, Unit = "pot", StockQuantity = 16, FarmerId = willowbrook.Id,
                ImageUrl = "https://images.unsplash.com/photo-1618375569909-3c8616cf7733?w=800&auto=format&fit=crop&q=80"
            }
        };
        db.Products.AddRange(products);
        await db.SaveChangesAsync();

        // ───────────────────────── Pickup slots ─────────────────────────
        foreach (var f in farmers.Where(f => f.IsApproved))
        {
            db.PickupSlots.AddRange(
                new PickupSlot { FarmerId = f.Id, Label = "Morning · 08:00 – 10:00", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(10, 0, 0) },
                new PickupSlot { FarmerId = f.Id, Label = "Midday · 10:00 – 12:00", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
                new PickupSlot { FarmerId = f.Id, Label = "Afternoon · 12:00 – 14:00", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(14, 0, 0) }
            );
        }
        await db.SaveChangesAsync();

        // ───────────────────────── Sample orders ─────────────────────────
        var sophia = customers[0];
        var daniel = customers[1];
        var amelia = customers[2];

        Product P(string name) => products.First(p => p.Name == name);

        async Task<Order> MakeOrder(User customer, User farmer, OrderStatus status, DateTime pickup, string window,
            string? note, params (Product p, int qty)[] lines)
        {
            var order = new Order
            {
                OrderNumber = $"ML-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
                CustomerId = customer.Id,
                FarmerId = farmer.Id,
                Status = status,
                OrderDate = pickup.AddDays(-2),
                PickupDate = pickup,
                PickupWindow = window,
                CustomerNote = note,
                StatusUpdatedAt = pickup.AddDays(-1)
            };
            foreach (var (p, qty) in lines)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Unit = p.Unit,
                    UnitPrice = p.Price,
                    Quantity = qty
                });
                order.TotalAmount += p.Price * qty;
            }
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return order;
        }

        await MakeOrder(sophia, willowbrook, OrderStatus.Completed, DateTime.Today.AddDays(-7), "Morning · 08:00 – 10:00",
            "Please pick the tomatoes on the firmer side.",
            (P("Heritage Vine Tomatoes"), 3), (P("Morning Leaf Spinach"), 2), (P("Garden Herb Bundle"), 1));

        await MakeOrder(sophia, thornbury, OrderStatus.ReadyForPickup, DateTime.Today.AddDays(1), "Morning · 08:00 – 10:00",
            "Slicing the sourdough for dinner tonight.",
            (P("Stone-Hearth Sourdough"), 2), (P("Butter Croissants"), 4));

        await MakeOrder(daniel, meadowcroft, OrderStatus.Accepted, DateTime.Today.AddDays(2), "Midday · 10:00 – 12:00",
            "Glass bottles will be returned at pickup.",
            (P("Cream-Top Whole Milk"), 2), (P("Free-Range Eggs"), 1), (P("Hand-Churned Farm Butter"), 1));

        await MakeOrder(daniel, stonegate, OrderStatus.Placed, DateTime.Today.AddDays(3), "Afternoon · 12:00 – 14:00",
            "Mangoes for a family gathering.",
            (P("Alphonso Mangoes"), 2), (P("Orchard Guavas"), 2));

        await MakeOrder(amelia, honeyfield, OrderStatus.Completed, DateTime.Today.AddDays(-5), "Morning · 08:00 – 10:00",
            null,
            (P("Wildflower Honey"), 1), (P("Golden Quince Preserve"), 1));

        await MakeOrder(amelia, willowbrook, OrderStatus.Completed, DateTime.Today.AddDays(-10), "Midday · 10:00 – 12:00",
            null,
            (P("Golden Baby Potatoes"), 2), (P("Sweet Carrots (Bunched)"), 2), (P("Heritage Vine Tomatoes"), 2));

        // ───────────────────────── Reviews ─────────────────────────
        db.Reviews.AddRange(
            new Review
            {
                ProductId = P("Heritage Vine Tomatoes").Id, CustomerId = sophia.Id, FarmerId = willowbrook.Id,
                Rating = 5, Comment = "The most flavourful tomatoes I have tasted in years — like my grandmother's garden.",
                ReviewDate = DateTime.Today.AddDays(-6),
                FarmerResponse = "Thank you, Sophia! They were especially good this week after the cool nights."
            },
            new Review
            {
                ProductId = P("Golden Baby Potatoes").Id, CustomerId = amelia.Id, FarmerId = willowbrook.Id,
                Rating = 5, Comment = "Buttery and sweet. Wonderful roasted with the herb bundle.",
                ReviewDate = DateTime.Today.AddDays(-9)
            },
            new Review
            {
                ProductId = P("Wildflower Honey").Id, CustomerId = amelia.Id, FarmerId = honeyfield.Id,
                Rating = 4, Comment = "Delicate floral flavour. The jar is almost too beautiful to open.",
                ReviewDate = DateTime.Today.AddDays(-4)
            },
            new Review
            {
                ProductId = P("Sweet Carrots (Bunched)").Id, CustomerId = sophia.Id, FarmerId = willowbrook.Id,
                Rating = 5, Comment = "So fresh the tops were still crisp. My children ate them raw like sweets.",
                ReviewDate = DateTime.Today.AddDays(-8)
            },
            new Review
            {
                ProductId = P("Alphonso Mangoes").Id, CustomerId = daniel.Id, FarmerId = stonegate.Id,
                Rating = 5, Comment = "Worth every rupee. Fragrant, honeyed and perfectly ripe on arrival.",
                ReviewDate = DateTime.Today.AddDays(-3)
            }
        );

        // ───────────────────────── Favorites ─────────────────────────
        db.Favorites.AddRange(
            new Favorite { CustomerId = sophia.Id, ProductId = P("Heritage Vine Tomatoes").Id },
            new Favorite { CustomerId = sophia.Id, ProductId = P("Stone-Hearth Sourdough").Id },
            new Favorite { CustomerId = sophia.Id, FarmerId = willowbrook.Id },
            new Favorite { CustomerId = daniel.Id, ProductId = P("Alphonso Mangoes").Id },
            new Favorite { CustomerId = daniel.Id, FarmerId = meadowcroft.Id },
            new Favorite { CustomerId = amelia.Id, ProductId = P("Wildflower Honey").Id }
        );

        // ───────────────────────── Announcements ─────────────────────────
        db.Announcements.AddRange(
            new Announcement
            {
                Title = "Autumn Harvest Festival — this Saturday",
                Message = "Clifton Green Farmers Market celebrates the harvest with live music, tasting stalls and a prize for the best-dressed basket. Markets open at 8:00 AM."
            },
            new Announcement
            {
                Title = "Pre-orders now open for the weekend",
                Message = "Reserve your favourites before Friday evening — popular items sell out before the bell rings."
            }
        );

        // ───────────────────────── Notifications ─────────────────────────
        db.Notifications.AddRange(
            new Notification
            {
                UserId = sophia.Id, Title = "Your basket is ready",
                Message = "Thornbury Bakehouse has marked order ML ready for pickup in the Morning window.",
                Type = "Order", LinkUrl = "/PreOrder/Orders", IsRead = false
            },
            new Notification
            {
                UserId = willowbrook.Id, Title = "New pre-order received",
                Message = "Sophia Whitmore placed a pre-order for 3 items.", Type = "Order", LinkUrl = "/Farmer/Orders"
            },
            new Notification
            {
                UserId = daniel.Id, Title = "Order accepted",
                Message = "Meadowcroft Dairy accepted your pre-order for pickup on market day.", Type = "Order", LinkUrl = "/PreOrder/Orders"
            }
        );

        // ───────────────────────── Platform report log ─────────────────────────
        db.Reports.Add(new Report
        {
            GeneratedBy = admin.Id,
            ReportType = "Platform Overview",
            Summary = "Initial platform baseline report generated at launch.",
            GeneratedAt = DateTime.UtcNow.AddDays(-1)
        });

        await db.SaveChangesAsync();
    }
}
