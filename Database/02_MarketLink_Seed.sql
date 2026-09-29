/* ═══════════════════════════════════════════════════════════════════
   MarketLink · eGreen Basket — Sample / Test Data
   Mirrors Data/DbSeeder.cs so the database can be loaded entirely from SQL.
   Run 01_MarketLink_Schema.sql first.
   Demo credentials (passwords hashed with PBKDF2):
     admin@marketlink.local    / Admin@123
     farmer@marketlink.local   / Farmer@123   (plus other growers)
     customer@marketlink.local / Customer@123 (plus other customers)
   ═══════════════════════════════════════════════════════════════════ */

USE [MarketLink];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- ── Users (10 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (1, N'Eleanor Hartwell', N'admin@marketlink.local', N'100000.Xt1bym6FcRCtGAYvSGSVIA==.ECsYjBEKZ7Mvcx6jpraYm0+79OmbXtCqj3sSHBCiXN8=', 0, N'+92 21 111 000 111', N'MarketLink House, Clifton, Karachi', NULL, NULL, NULL, NULL, NULL, 1, 1, 12, N'2026-09-27T09:51:14.8076776');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (2, N'Harold Green', N'farmer@marketlink.local', N'100000.bnByTc8DDEKXdyZXKOryLQ==.Ckdv+dq4rttjjv9Knb3H0ULFl+cISTge/lUJLqU3nkk=', 1, N'+92 300 222 3344', N'Willowbrook Lane, Malir, Karachi', N'Willowbrook Farm', N'Four generations of market gardeners. Willowbrook is known for heritage tomatoes, crisp greens and the sweetest strawberries of the season — grown slowly, picked ripe, carried to market the same morning.', NULL, 24.8923, 67.0822, 1, 1, 12, N'2026-09-27T09:51:14.8407402');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (3, N'Beatrice Ashford', N'beatrice@marketlink.local', N'100000.8tARUibThx1ANoFUcjQ24w==.H8ytSv1J7LEDQDtjJlk8z11WbRMx/Yv53UhW+ZK8AnA=', 1, N'+92 301 555 4488', N'Stonegate Orchard, Gadap, Karachi', N'Stonegate Orchard', N'A quiet orchard of mango, guava and citrus trees. We press our own juices and bottle orchard honey each autumn.', NULL, 25.0211, 67.0931, 1, 1, 18, N'2026-09-27T09:51:14.8775189');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (4, N'James Holloway', N'james@marketlink.local', N'100000.P5AUFNMjV8j3tq7SqvtX5A==.zWef1ONU87B5DTLMZu/KGaZE0rubjg7j51BFbpAu8yI=', 1, N'+92 333 777 2211', N'Meadowcroft Dairy, Super Highway, Karachi', N'Meadowcroft Dairy', N'Pasture-fed cows, glass bottles and butter churned by hand. Our cream-top milk barely makes it past noon on market days.', NULL, 24.9612, 67.1204, 1, 1, 10, N'2026-09-27T09:51:14.9031436');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (5, N'Adelaide Thornbury', N'adelaide@marketlink.local', N'100000.DyxCyMwbuxvsnmVvawGRUQ==.5fg6KkfI1RxSdIb5oQXY3TIAhyW7ygUzuIJdfDKpPEk=', 1, N'+92 345 888 6677', N'Thornbury Bakehouse, Zamzama, Karachi', N'Thornbury Bakehouse', N'Slow-fermented sourdough, honey oat loaves and butter pastries baked before dawn in a stone hearth oven.', NULL, 24.8134, 67.0301, 1, 1, 24, N'2026-09-27T09:51:14.9764315');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (6, N'Oscar Whitfield', N'oscar@marketlink.local', N'100000.7VQOhsVSBtUY20JnhCOVJQ==.KaZ/oMzjWDLD8HAi84j+Yroc8y2x3gFymud6072fO/4=', 1, N'+92 302 444 9900', N'Honeyfield Apiary, Hub River Road, Karachi', N'Honeyfield Apiary & Preserves', N'Wildflower honey from hives kept at the edge of the countryside, plus small-batch jams and pickles.', NULL, 24.8745, 66.9822, 1, 1, 12, N'2026-09-27T09:51:15.0156801');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (7, N'Clara Bennett', N'clara@marketlink.local', N'100000.tx49v9+vCvEwixgbOy72ZA==.nCaZYiuvwEDXsg/HUlkFFSlPnuIdsb88nIHr8eoMHMc=', 1, N'+92 311 222 3311', N'Green Acres Estate, North Nazimabad, Karachi', N'Green Acres Estate', N'Herbs, salad leaves and edible flowers raised in raised beds under morning light.', NULL, 24.9302, 67.0402, 0, 1, 12, N'2026-09-27T09:51:15.0457179');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (8, N'Sophia Whitmore', N'customer@marketlink.local', N'100000.uOOM4LzRFgDYfQD4hWKXBw==.OvewI5pIXU41CfgGvcUbz/HjJb0AKdhkzpLIyxrcrx4=', 2, N'+92 321 123 4567', N'12 Sea View Apartments, Clifton, Karachi', NULL, NULL, NULL, NULL, NULL, 0, 1, 12, N'2026-09-27T09:51:15.0887911');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (9, N'Daniel Reyes', N'daniel@marketlink.local', N'100000.69tnie50SOmEQi6JeqANvg==.dnaRcln3LkxRr9oQzShjaPAbt8zcuO7SuXfN+IKu/qM=', 2, N'+92 333 987 6543', N'45 Palm Avenue, DHA Phase 6, Karachi', NULL, NULL, NULL, NULL, NULL, 0, 1, 12, N'2026-09-27T09:51:15.1449652');
INSERT INTO [Users] ([Id], [FullName], [Email], [PasswordHash], [Role], [Phone], [Address], [StallName], [Bio], [ProfileImageUrl], [Latitude], [Longitude], [IsApproved], [IsActive], [OrderCutoffHours], [CreatedAt]) VALUES (10, N'Amelia Stone', N'amelia@marketlink.local', N'100000.gZrLA6Tb9kvuLhdQ1WJIWw==.PmTZOUX9iWAfTZfV2xPv1soYrt7tEeGccQUl0cNYrjs=', 2, N'+92 300 456 7890', N'8 Garden Terrace, Bath Island, Karachi', NULL, NULL, NULL, NULL, NULL, 0, 1, 12, N'2026-09-27T09:51:15.2290798');
SET IDENTITY_INSERT [Users] OFF;
DBCC CHECKIDENT ('[Users]', RESEED, 10);
GO

-- ── Markets (3 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Markets] ON;
INSERT INTO [Markets] ([Id], [Name], [Description], [Address], [City], [Latitude], [Longitude], [MapProvider], [OperatingDays], [OpenTime], [CloseTime], [ImageUrl], [IsActive]) VALUES (1, N'Clifton Green Farmers Market', N'The grand old Saturday market beneath the rain trees — heritage produce, cut flowers and hearth breads.', N'Clifton Park, Block 5, Clifton', N'Karachi', 24.8153, 67.0281, N'OpenStreetMap', N'Saturday,Sunday', N'08:00:00', N'14:00:00', N'images/market-clifton.svg', 1);
INSERT INTO [Markets] ([Id], [Name], [Description], [Address], [City], [Latitude], [Longitude], [MapProvider], [OperatingDays], [OpenTime], [CloseTime], [ImageUrl], [IsActive]) VALUES (2, N'Old Town Harvest Market', N'A weekday market in the old quarter, famous for fruit, dairy and preserves from the outlying estates.', N'M.A. Jinnah Road, Saddar', N'Karachi', 24.8612, 67.0098, N'OpenStreetMap', N'Wednesday,Friday', N'09:00:00', N'15:00:00', N'images/market-oldtown.svg', 1);
INSERT INTO [Markets] ([Id], [Name], [Description], [Address], [City], [Latitude], [Longitude], [MapProvider], [OperatingDays], [OpenTime], [CloseTime], [ImageUrl], [IsActive]) VALUES (3, N'Riverside Artisan Market', N'A Sunday morning gathering by the river — organic greens, small-batch baked goods and live acoustic music.', N'Riverside Promenade, Korangi Creek', N'Karachi', 24.8231, 67.1157, N'OpenStreetMap', N'Sunday', N'07:30:00', N'12:30:00', N'images/market-riverside.svg', 1);
SET IDENTITY_INSERT [Markets] OFF;
DBCC CHECKIDENT ('[Markets]', RESEED, 3);
GO

-- ── Categories (8 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] ([Id], [Name], [Description], [ImageUrl], [SortOrder], [IsActive]) VALUES (1, N'Vegetables', N'Harvested this morning', N'images/cat-vegetables.jpg', 1, 1);
INSERT INTO [Categories] ([Id], [Name], [Description], [ImageUrl], [SortOrder], [IsActive]) VALUES (2, N'Fruits', N'Tree-ripened & seasonal', N'images/cat-fruits.jpg', 2, 1);
INSERT INTO [Categories] ([Id], [Name], [Description], [ImageUrl], [SortOrder], [IsActive]) VALUES (3, N'Dairy & Eggs', N'Milk, butter & eggs', N'images/cat-dairy.jpg', 3, 1);
INSERT INTO [Categories] ([Id], [Name], [Description], [ImageUrl], [SortOrder], [IsActive]) VALUES (4, N'Baked Goods', N'Stone-hearth baked', N'images/cat-bakery.jpg', 4, 1);
INSERT INTO [Categories] ([Id], [Name], [Description], [ImageUrl], [SortOrder], [IsActive]) VALUES (5, N'Herbs & Greens', N'Aromatic & fresh', N'images/cat-herbs.jpg', 5, 1);
INSERT INTO [Categories] ([Id], [Name], [Description], [ImageUrl], [SortOrder], [IsActive]) VALUES (6, N'Honey & Preserves', N'Golden & preserved', N'images/cat-honey.jpg', 6, 1);
INSERT INTO [Categories] ([Id], [Name], [Description], [ImageUrl], [SortOrder], [IsActive]) VALUES (7, N'Grains & Pulses', N'Stone-milled staples', N'images/cat-grains.jpg', 7, 1);
INSERT INTO [Categories] ([Id], [Name], [Description], [ImageUrl], [SortOrder], [IsActive]) VALUES (8, N'Flowers & Plants', N'Cut & potted', N'images/cat-flowers.jpg', 8, 1);
SET IDENTITY_INSERT [Categories] OFF;
DBCC CHECKIDENT ('[Categories]', RESEED, 8);
GO

-- ── MarketFarmers (7 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [MarketFarmers] ON;
INSERT INTO [MarketFarmers] ([Id], [MarketId], [FarmerId], [StallLocation]) VALUES (1, 1, 2, N'Row A · Stall 3');
INSERT INTO [MarketFarmers] ([Id], [MarketId], [FarmerId], [StallLocation]) VALUES (2, 1, 5, N'Row B · Stall 1');
INSERT INTO [MarketFarmers] ([Id], [MarketId], [FarmerId], [StallLocation]) VALUES (3, 1, 4, N'Row A · Stall 7');
INSERT INTO [MarketFarmers] ([Id], [MarketId], [FarmerId], [StallLocation]) VALUES (4, 2, 3, N'Row C · Stall 2');
INSERT INTO [MarketFarmers] ([Id], [MarketId], [FarmerId], [StallLocation]) VALUES (5, 2, 6, N'Row C · Stall 5');
INSERT INTO [MarketFarmers] ([Id], [MarketId], [FarmerId], [StallLocation]) VALUES (6, 3, 2, N'Row A · Stall 1');
INSERT INTO [MarketFarmers] ([Id], [MarketId], [FarmerId], [StallLocation]) VALUES (7, 3, 5, N'Row B · Stall 4');
SET IDENTITY_INSERT [MarketFarmers] OFF;
DBCC CHECKIDENT ('[MarketFarmers]', RESEED, 7);
GO

-- ── Products (21 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Products] ON;
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (1, N'Heritage Vine Tomatoes', N'Deep-ribbed heirloom tomatoes ripened on the vine. Rich, sweet and perfect for a summer salad.', 240.0, N'kg', 58, N'https://images.unsplash.com/photo-1592924357228-91a4daadcfea?w=800&auto=format&fit=crop&q=80', 2, 1, 1, 0, 1, 60, 1, N'2026-09-27T09:51:15.7279929', N'2026-09-27T09:51:15.7279930');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (2, N'Morning Leaf Spinach', N'Tender young spinach picked at first light, still cool from the field.', 120.0, N'bunch', 45, N'https://images.unsplash.com/photo-1576045057995-568f588f82fb?w=800&auto=format&fit=crop&q=80', 2, 1, 1, 0, 1, 45, 1, N'2026-09-27T09:51:15.7285456', N'2026-09-27T09:51:15.7285456');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (3, N'Golden Baby Potatoes', N'Buttery yellow potatoes, dug the evening before market day.', 160.0, N'kg', 80, N'https://images.unsplash.com/photo-1518977676601-b53f82aba655?w=800&auto=format&fit=crop&q=80', 2, 1, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285480', N'2026-09-27T09:51:15.7285480');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (4, N'Sweet Carrots (Bunched)', N'Crunchy, sweet carrots with their tops on — evidence of true freshness.', 140.0, N'bunch', 50, N'https://images.unsplash.com/photo-1445282768818-728615cc910a?w=800&auto=format&fit=crop&q=80', 2, 1, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285485', N'2026-09-27T09:51:15.7285485');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (5, N'Cherry Tomato Medley', N'Ruby, amber and chocolate cherry tomatoes in a paper pint box.', 300.0, N'box', 30, N'https://images.unsplash.com/photo-1546470427-e26264be0b0d?w=800&auto=format&fit=crop&q=80', 2, 1, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285488', N'2026-09-27T09:51:15.7285489');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (6, N'Alphonso Mangoes', N'The celebrated Alphonso — honeyed, fragrant and utterly indulgent. Limited harvest.', 650.0, N'dozen', 24, N'https://images.unsplash.com/photo-1553279768-865429fa0078?w=800&auto=format&fit=crop&q=80', 3, 2, 1, 0, 1, 24, 1, N'2026-09-27T09:51:15.7285492', N'2026-09-27T09:51:15.7285492');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (7, N'Orchard Guavas', N'Fragrant pink-fleshed guavas, hand-picked from Stonegate''s oldest trees.', 220.0, N'kg', 40, N'https://images.unsplash.com/photo-1536511132770-e5058c7e8c46?w=800&auto=format&fit=crop&q=80', 3, 2, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285503', N'2026-09-27T09:51:15.7285503');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (8, N'Valencia Oranges', N'Juicy, thin-skinned oranges — half a glass of sunshine per fruit.', 180.0, N'kg', 55, N'https://images.unsplash.com/photo-1547514701-42782101795e?w=800&auto=format&fit=crop&q=80', 3, 2, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285507', N'2026-09-27T09:51:15.7285507');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (9, N'Cream-Top Whole Milk', N'Non-homogenised whole milk in a glass bottle, with a ribbon of cream on top.', 260.0, N'liter', 35, N'https://images.unsplash.com/photo-1563636619-e9143da7973b?w=800&auto=format&fit=crop&q=80', 4, 3, 1, 0, 1, 35, 1, N'2026-09-27T09:51:15.7285512', N'2026-09-27T09:51:15.7285512');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (10, N'Hand-Churned Farm Butter', N'Cultured butter churned in small batches, flecked with sea salt.', 420.0, N'block', 20, N'https://images.unsplash.com/photo-1589985270826-4b7bb135bc9d?w=800&auto=format&fit=crop&q=80', 4, 3, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285520', N'2026-09-27T09:51:15.7285520');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (11, N'Free-Range Eggs', N'A dozen eggs from hens that wander the meadow all day.', 320.0, N'dozen', 42, N'https://images.unsplash.com/photo-1582722872445-44dc5f7e3c8f?w=800&auto=format&fit=crop&q=80', 4, 3, 1, 0, 1, 42, 1, N'2026-09-27T09:51:15.7285526', N'2026-09-27T09:51:15.7285526');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (12, N'Meadowcroft Yogurt', N'Thick, tangy set yogurt made the traditional way in clay-lined pots.', 280.0, N'jar', 18, N'https://images.unsplash.com/photo-1488477181946-6428a0291777?w=800&auto=format&fit=crop&q=80', 4, 3, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285531', N'2026-09-27T09:51:15.7285531');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (13, N'Stone-Hearth Sourdough', N'A 36-hour fermented loaf with a burnished crust and open, creamy crumb.', 350.0, N'loaf', 25, N'https://images.unsplash.com/photo-1509440159596-0249088772ff?w=800&auto=format&fit=crop&q=80', 5, 4, 1, 0, 1, 25, 1, N'2026-09-27T09:51:15.7285534', N'2026-09-27T09:51:15.7285535');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (14, N'Honey Oat Loaf', N'Soft, lightly sweetened with Honeyfield wildflower honey and rolled oats.', 280.0, N'loaf', 20, N'https://images.unsplash.com/photo-1586444248902-2f64eddc13df?w=800&auto=format&fit=crop&q=80', 5, 4, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285561', N'2026-09-27T09:51:15.7285562');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (15, N'Butter Croissants', N'Laminated by hand over two days. Shatteringly flaky, deeply buttery.', 180.0, N'piece', 36, N'https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=800&auto=format&fit=crop&q=80', 5, 4, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285567', N'2026-09-27T09:51:15.7285567');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (16, N'Cinnamon Morning Buns', N'Brioche swirls with Ceylon cinnamon and a whisper of orange zest.', 160.0, N'piece', 0, N'https://images.unsplash.com/photo-1509365465985-25d11c17e812?w=800&auto=format&fit=crop&q=80', 5, 4, 1, 1, 0, 0, 1, N'2026-09-27T09:51:15.7285571', N'2026-09-27T09:51:15.7285571');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (17, N'Garden Herb Bundle', N'Basil, mint, coriander and dill tied with garden twine.', 150.0, N'bundle', 40, N'https://images.unsplash.com/photo-1466637574441-749b8f19452f?w=800&auto=format&fit=crop&q=80', 2, 5, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285756', N'2026-09-27T09:51:15.7285756');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (18, N'Wildflower Honey', N'Raw, unfiltered honey drawn from hives at the edge of the countryside.', 780.0, N'jar', 30, N'https://images.unsplash.com/photo-1587049352846-4a222e784d38?w=800&auto=format&fit=crop&q=80', 6, 6, 1, 0, 1, 30, 1, N'2026-09-27T09:51:15.7285765', N'2026-09-27T09:51:15.7285765');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (19, N'Golden Quince Preserve', N'Slow-cooked quince set to a jewel-like jelly. Wonderful with buttered sourdough.', 520.0, N'jar', 22, N'https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=800&auto=format&fit=crop&q=80', 6, 6, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285770', N'2026-09-27T09:51:15.7285770');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (20, N'Stone-Milled Whole Wheat', N'Nutty, nutrient-rich flour milled slowly between stone wheels.', 240.0, N'kg', 48, N'https://images.unsplash.com/photo-1574323347407-f5e1ad6d020b?w=800&auto=format&fit=crop&q=80', 3, 7, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285774', N'2026-09-27T09:51:15.7285775');
INSERT INTO [Products] ([Id], [Name], [Description], [Price], [Unit], [StockQuantity], [ImageUrl], [FarmerId], [CategoryId], [IsAvailable], [IsSoldOut], [IsRecurringWeekly], [DefaultWeeklyQuantity], [IsVisible], [CreatedAt], [UpdatedAt]) VALUES (21, N'Fresh Basil Pot', N'A living basil plant in a terracotta pot — snip leaves as you need them.', 380.0, N'pot', 16, N'https://images.unsplash.com/photo-1618375569909-3c8616cf7733?w=800&auto=format&fit=crop&q=80', 2, 8, 1, 0, 0, 0, 1, N'2026-09-27T09:51:15.7285779', N'2026-09-27T09:51:15.7285779');
SET IDENTITY_INSERT [Products] OFF;
DBCC CHECKIDENT ('[Products]', RESEED, 21);
GO

-- ── PickupSlots (15 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [PickupSlots] ON;
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (1, 2, N'Morning · 08:00 – 10:00', N'08:00:00', N'10:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (2, 2, N'Midday · 10:00 – 12:00', N'10:00:00', N'12:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (3, 2, N'Afternoon · 12:00 – 14:00', N'12:00:00', N'14:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (4, 3, N'Morning · 08:00 – 10:00', N'08:00:00', N'10:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (5, 3, N'Midday · 10:00 – 12:00', N'10:00:00', N'12:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (6, 3, N'Afternoon · 12:00 – 14:00', N'12:00:00', N'14:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (7, 4, N'Morning · 08:00 – 10:00', N'08:00:00', N'10:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (8, 4, N'Midday · 10:00 – 12:00', N'10:00:00', N'12:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (9, 4, N'Afternoon · 12:00 – 14:00', N'12:00:00', N'14:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (10, 5, N'Morning · 08:00 – 10:00', N'08:00:00', N'10:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (11, 5, N'Midday · 10:00 – 12:00', N'10:00:00', N'12:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (12, 5, N'Afternoon · 12:00 – 14:00', N'12:00:00', N'14:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (13, 6, N'Morning · 08:00 – 10:00', N'08:00:00', N'10:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (14, 6, N'Midday · 10:00 – 12:00', N'10:00:00', N'12:00:00', 1);
INSERT INTO [PickupSlots] ([Id], [FarmerId], [Label], [StartTime], [EndTime], [IsActive]) VALUES (15, 6, N'Afternoon · 12:00 – 14:00', N'12:00:00', N'14:00:00', 1);
SET IDENTITY_INSERT [PickupSlots] OFF;
DBCC CHECKIDENT ('[PickupSlots]', RESEED, 15);
GO

-- ── Orders (7 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Orders] ON;
INSERT INTO [Orders] ([Id], [OrderNumber], [CustomerId], [FarmerId], [Status], [OrderDate], [PickupDate], [PickupWindow], [TotalAmount], [CustomerNote], [FarmerNote], [StatusUpdatedAt], [CancelledAt]) VALUES (1, N'ML-20260927-8574', 8, 2, 4, N'2026-09-18T00:00:00', N'2026-09-20T00:00:00', N'Morning · 08:00 – 10:00', 1110.0, N'Please pick the tomatoes on the firmer side.', NULL, N'2026-09-19T00:00:00', NULL);
INSERT INTO [Orders] ([Id], [OrderNumber], [CustomerId], [FarmerId], [Status], [OrderDate], [PickupDate], [PickupWindow], [TotalAmount], [CustomerNote], [FarmerNote], [StatusUpdatedAt], [CancelledAt]) VALUES (2, N'ML-20260927-5888', 8, 5, 3, N'2026-09-26T00:00:00', N'2026-09-28T00:00:00', N'Morning · 08:00 – 10:00', 1420.0, N'Slicing the sourdough for dinner tonight.', NULL, N'2026-09-27T00:00:00', NULL);
INSERT INTO [Orders] ([Id], [OrderNumber], [CustomerId], [FarmerId], [Status], [OrderDate], [PickupDate], [PickupWindow], [TotalAmount], [CustomerNote], [FarmerNote], [StatusUpdatedAt], [CancelledAt]) VALUES (3, N'ML-20260927-4249', 9, 4, 1, N'2026-09-27T00:00:00', N'2026-09-29T00:00:00', N'Midday · 10:00 – 12:00', 1260.0, N'Glass bottles will be returned at pickup.', NULL, N'2026-09-28T00:00:00', NULL);
INSERT INTO [Orders] ([Id], [OrderNumber], [CustomerId], [FarmerId], [Status], [OrderDate], [PickupDate], [PickupWindow], [TotalAmount], [CustomerNote], [FarmerNote], [StatusUpdatedAt], [CancelledAt]) VALUES (4, N'ML-20260927-7373', 9, 3, 0, N'2026-09-28T00:00:00', N'2026-09-30T00:00:00', N'Afternoon · 12:00 – 14:00', 1740.0, N'Mangoes for a family gathering.', NULL, N'2026-09-29T00:00:00', NULL);
INSERT INTO [Orders] ([Id], [OrderNumber], [CustomerId], [FarmerId], [Status], [OrderDate], [PickupDate], [PickupWindow], [TotalAmount], [CustomerNote], [FarmerNote], [StatusUpdatedAt], [CancelledAt]) VALUES (5, N'ML-20260927-1150', 10, 6, 4, N'2026-09-20T00:00:00', N'2026-09-22T00:00:00', N'Morning · 08:00 – 10:00', 1300.0, NULL, NULL, N'2026-09-21T00:00:00', NULL);
INSERT INTO [Orders] ([Id], [OrderNumber], [CustomerId], [FarmerId], [Status], [OrderDate], [PickupDate], [PickupWindow], [TotalAmount], [CustomerNote], [FarmerNote], [StatusUpdatedAt], [CancelledAt]) VALUES (6, N'ML-20260927-5611', 10, 2, 4, N'2026-09-15T00:00:00', N'2026-09-17T00:00:00', N'Midday · 10:00 – 12:00', 1080.0, NULL, NULL, N'2026-09-16T00:00:00', NULL);
INSERT INTO [Orders] ([Id], [OrderNumber], [CustomerId], [FarmerId], [Status], [OrderDate], [PickupDate], [PickupWindow], [TotalAmount], [CustomerNote], [FarmerNote], [StatusUpdatedAt], [CancelledAt]) VALUES (7, N'ML-20260927-6486', 9, 2, 0, N'2026-09-27T09:52:41.5682333', N'2026-10-03T00:00:00', N'Morning · 08:00 – 10:00', 480.0, N'Please pick firm tomatoes', NULL, NULL, NULL);
SET IDENTITY_INSERT [Orders] OFF;
DBCC CHECKIDENT ('[Orders]', RESEED, 7);
GO

-- ── OrderItems (16 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [OrderItems] ON;
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (1, 1, 1, N'Heritage Vine Tomatoes', N'kg', 240.0, 3);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (2, 1, 2, N'Morning Leaf Spinach', N'bunch', 120.0, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (3, 1, 17, N'Garden Herb Bundle', N'bundle', 150.0, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (4, 2, 13, N'Stone-Hearth Sourdough', N'loaf', 350.0, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (5, 2, 15, N'Butter Croissants', N'piece', 180.0, 4);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (6, 3, 9, N'Cream-Top Whole Milk', N'liter', 260.0, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (7, 3, 11, N'Free-Range Eggs', N'dozen', 320.0, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (8, 3, 10, N'Hand-Churned Farm Butter', N'block', 420.0, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (9, 4, 6, N'Alphonso Mangoes', N'dozen', 650.0, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (10, 4, 7, N'Orchard Guavas', N'kg', 220.0, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (11, 5, 18, N'Wildflower Honey', N'jar', 780.0, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (12, 5, 19, N'Golden Quince Preserve', N'jar', 520.0, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (13, 6, 3, N'Golden Baby Potatoes', N'kg', 160.0, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (14, 6, 4, N'Sweet Carrots (Bunched)', N'bunch', 140.0, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (15, 6, 1, N'Heritage Vine Tomatoes', N'kg', 240.0, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ProductId], [ProductName], [Unit], [UnitPrice], [Quantity]) VALUES (16, 7, 1, N'Heritage Vine Tomatoes', N'kg', 240.0, 2);
SET IDENTITY_INSERT [OrderItems] OFF;
DBCC CHECKIDENT ('[OrderItems]', RESEED, 16);
GO

-- ── Reviews (5 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Reviews] ON;
INSERT INTO [Reviews] ([Id], [ProductId], [CustomerId], [FarmerId], [Rating], [Comment], [ReviewDate], [FarmerResponse], [ResponseDate], [IsVisible]) VALUES (1, 1, 8, 2, 5, N'The most flavourful tomatoes I have tasted in years — like my grandmother''s garden.', N'2026-09-21T00:00:00', N'Thank you, Sophia! They were especially good this week after the cool nights.', NULL, 1);
INSERT INTO [Reviews] ([Id], [ProductId], [CustomerId], [FarmerId], [Rating], [Comment], [ReviewDate], [FarmerResponse], [ResponseDate], [IsVisible]) VALUES (2, 3, 10, 2, 5, N'Buttery and sweet. Wonderful roasted with the herb bundle.', N'2026-09-18T00:00:00', NULL, NULL, 1);
INSERT INTO [Reviews] ([Id], [ProductId], [CustomerId], [FarmerId], [Rating], [Comment], [ReviewDate], [FarmerResponse], [ResponseDate], [IsVisible]) VALUES (3, 18, 10, 6, 4, N'Delicate floral flavour. The jar is almost too beautiful to open.', N'2026-09-23T00:00:00', NULL, NULL, 1);
INSERT INTO [Reviews] ([Id], [ProductId], [CustomerId], [FarmerId], [Rating], [Comment], [ReviewDate], [FarmerResponse], [ResponseDate], [IsVisible]) VALUES (4, 4, 8, 2, 5, N'So fresh the tops were still crisp. My children ate them raw like sweets.', N'2026-09-19T00:00:00', NULL, NULL, 1);
INSERT INTO [Reviews] ([Id], [ProductId], [CustomerId], [FarmerId], [Rating], [Comment], [ReviewDate], [FarmerResponse], [ResponseDate], [IsVisible]) VALUES (5, 6, 9, 3, 5, N'Worth every rupee. Fragrant, honeyed and perfectly ripe on arrival.', N'2026-09-24T00:00:00', NULL, NULL, 1);
SET IDENTITY_INSERT [Reviews] OFF;
DBCC CHECKIDENT ('[Reviews]', RESEED, 5);
GO

-- ── Favorites (6 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Favorites] ON;
INSERT INTO [Favorites] ([Id], [CustomerId], [ProductId], [FarmerId], [CreatedAt]) VALUES (1, 8, 1, NULL, N'2026-09-27T09:51:16.0689506');
INSERT INTO [Favorites] ([Id], [CustomerId], [ProductId], [FarmerId], [CreatedAt]) VALUES (2, 8, 13, NULL, N'2026-09-27T09:51:16.0690152');
INSERT INTO [Favorites] ([Id], [CustomerId], [ProductId], [FarmerId], [CreatedAt]) VALUES (3, 8, NULL, 2, N'2026-09-27T09:51:16.0690170');
INSERT INTO [Favorites] ([Id], [CustomerId], [ProductId], [FarmerId], [CreatedAt]) VALUES (4, 9, 6, NULL, N'2026-09-27T09:51:16.0690392');
INSERT INTO [Favorites] ([Id], [CustomerId], [ProductId], [FarmerId], [CreatedAt]) VALUES (5, 9, NULL, 4, N'2026-09-27T09:51:16.0690397');
INSERT INTO [Favorites] ([Id], [CustomerId], [ProductId], [FarmerId], [CreatedAt]) VALUES (6, 10, 18, NULL, N'2026-09-27T09:51:16.0690399');
SET IDENTITY_INSERT [Favorites] OFF;
DBCC CHECKIDENT ('[Favorites]', RESEED, 6);
GO

-- ── Notifications (4 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Notifications] ON;
INSERT INTO [Notifications] ([Id], [UserId], [Title], [Message], [Type], [LinkUrl], [IsRead], [CreatedAt]) VALUES (1, 8, N'Your basket is ready', N'Thornbury Bakehouse has marked order ML ready for pickup in the Morning window.', N'Order', N'/PreOrder/Orders', 0, N'2026-09-27T09:51:16.1010868');
INSERT INTO [Notifications] ([Id], [UserId], [Title], [Message], [Type], [LinkUrl], [IsRead], [CreatedAt]) VALUES (2, 2, N'New pre-order received', N'Sophia Whitmore placed a pre-order for 3 items.', N'Order', N'/Farmer/Orders', 0, N'2026-09-27T09:51:16.1012471');
INSERT INTO [Notifications] ([Id], [UserId], [Title], [Message], [Type], [LinkUrl], [IsRead], [CreatedAt]) VALUES (3, 9, N'Order accepted', N'Meadowcroft Dairy accepted your pre-order for pickup on market day.', N'Order', N'/PreOrder/Orders', 0, N'2026-09-27T09:51:16.1012477');
INSERT INTO [Notifications] ([Id], [UserId], [Title], [Message], [Type], [LinkUrl], [IsRead], [CreatedAt]) VALUES (4, 2, N'New pre-order received', N'Daniel Reyes placed pre-order ML-20260927-6486 for pickup on Saturday, 3 Oct.', N'Order', N'/Farmer/Orders', 0, N'2026-09-27T09:52:41.5872605');
SET IDENTITY_INSERT [Notifications] OFF;
DBCC CHECKIDENT ('[Notifications]', RESEED, 4);
GO

-- ── Announcements (2 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Announcements] ON;
INSERT INTO [Announcements] ([Id], [Title], [Message], [IsActive], [CreatedAt]) VALUES (1, N'Autumn Harvest Festival — this Saturday', N'Clifton Green Farmers Market celebrates the harvest with live music, tasting stalls and a prize for the best-dressed basket. Markets open at 8:00 AM.', 1, N'2026-09-27T09:51:16.0917586');
INSERT INTO [Announcements] ([Id], [Title], [Message], [IsActive], [CreatedAt]) VALUES (2, N'Pre-orders now open for the weekend', N'Reserve your favourites before Friday evening — popular items sell out before the bell rings.', 1, N'2026-09-27T09:51:16.0918170');
SET IDENTITY_INSERT [Announcements] OFF;
DBCC CHECKIDENT ('[Announcements]', RESEED, 2);
GO

-- ── Reports (2 rows) ──────────────────────────────────────
SET IDENTITY_INSERT [Reports] ON;
INSERT INTO [Reports] ([Id], [GeneratedBy], [ReportType], [Summary], [GeneratedAt]) VALUES (1, 1, N'Platform Overview', N'Initial platform baseline report generated at launch.', N'2026-09-26T09:51:16.1177838');
INSERT INTO [Reports] ([Id], [GeneratedBy], [ReportType], [Summary], [GeneratedAt]) VALUES (2, 1, N'Sales Report 2026-08-27 → 2026-09-27', N'Orders: 5 · Revenue: Rs 3,490 · Markets: 2', N'2026-09-27T09:52:28.2972101');
SET IDENTITY_INSERT [Reports] OFF;
DBCC CHECKIDENT ('[Reports]', RESEED, 2);
GO

PRINT 'MarketLink sample data loaded successfully.';
GO