/* ═══════════════════════════════════════════════════════════════════
   MarketLink · eGreen Basket — Database Schema (Microsoft SQL Server)
   Generated from the EF Core migration 'InitialCreate'.
   Creates the [MarketLink] database and all tables, keys and indexes.
   Run this script first, then 02_MarketLink_Seed.sql (optional sample data).
   ═══════════════════════════════════════════════════════════════════ */

IF DB_ID(N'MarketLink') IS NULL
    CREATE DATABASE [MarketLink];
GO
USE [MarketLink];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Announcements] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(150) NOT NULL,
        [Message] nvarchar(1000) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Announcements] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(60) NOT NULL,
        [Description] nvarchar(300) NULL,
        [ImageUrl] nvarchar(300) NULL,
        [SortOrder] int NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Markets] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(120) NOT NULL,
        [Description] nvarchar(800) NULL,
        [Address] nvarchar(250) NOT NULL,
        [City] nvarchar(80) NOT NULL,
        [Latitude] float NOT NULL,
        [Longitude] float NOT NULL,
        [MapProvider] nvarchar(30) NOT NULL,
        [OperatingDays] nvarchar(80) NOT NULL,
        [OpenTime] time NOT NULL,
        [CloseTime] time NOT NULL,
        [ImageUrl] nvarchar(300) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Markets] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [FullName] nvarchar(80) NOT NULL,
        [Email] nvarchar(120) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [Role] int NOT NULL,
        [Phone] nvarchar(30) NULL,
        [Address] nvarchar(250) NULL,
        [StallName] nvarchar(120) NULL,
        [Bio] nvarchar(1000) NULL,
        [ProfileImageUrl] nvarchar(300) NULL,
        [Latitude] float NULL,
        [Longitude] float NULL,
        [IsApproved] bit NOT NULL,
        [IsActive] bit NOT NULL,
        [OrderCutoffHours] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [MarketFarmers] (
        [Id] int NOT NULL IDENTITY,
        [MarketId] int NOT NULL,
        [FarmerId] int NOT NULL,
        [StallLocation] nvarchar(120) NULL,
        CONSTRAINT [PK_MarketFarmers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MarketFarmers_Markets_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [Markets] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_MarketFarmers_Users_FarmerId] FOREIGN KEY ([FarmerId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Notifications] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Title] nvarchar(120) NOT NULL,
        [Message] nvarchar(500) NOT NULL,
        [Type] nvarchar(40) NOT NULL,
        [LinkUrl] nvarchar(200) NULL,
        [IsRead] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Orders] (
        [Id] int NOT NULL IDENTITY,
        [OrderNumber] nvarchar(24) NOT NULL,
        [CustomerId] int NOT NULL,
        [FarmerId] int NOT NULL,
        [Status] int NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [PickupDate] datetime2 NOT NULL,
        [PickupWindow] nvarchar(60) NOT NULL,
        [TotalAmount] decimal(10,2) NOT NULL,
        [CustomerNote] nvarchar(600) NULL,
        [FarmerNote] nvarchar(600) NULL,
        [StatusUpdatedAt] datetime2 NULL,
        [CancelledAt] datetime2 NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_Users_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Orders_Users_FarmerId] FOREIGN KEY ([FarmerId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [PickupSlots] (
        [Id] int NOT NULL IDENTITY,
        [FarmerId] int NOT NULL,
        [Label] nvarchar(60) NOT NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_PickupSlots] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PickupSlots_Users_FarmerId] FOREIGN KEY ([FarmerId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(120) NOT NULL,
        [Description] nvarchar(1500) NULL,
        [Price] decimal(10,2) NOT NULL,
        [Unit] nvarchar(20) NOT NULL,
        [StockQuantity] int NOT NULL,
        [ImageUrl] nvarchar(300) NULL,
        [FarmerId] int NOT NULL,
        [CategoryId] int NOT NULL,
        [IsAvailable] bit NOT NULL,
        [IsSoldOut] bit NOT NULL,
        [IsRecurringWeekly] bit NOT NULL,
        [DefaultWeeklyQuantity] int NOT NULL,
        [IsVisible] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Products_Users_FarmerId] FOREIGN KEY ([FarmerId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Reports] (
        [Id] int NOT NULL IDENTITY,
        [GeneratedBy] int NOT NULL,
        [ReportType] nvarchar(80) NOT NULL,
        [Summary] nvarchar(2000) NULL,
        [GeneratedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Reports] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Reports_Users_GeneratedBy] FOREIGN KEY ([GeneratedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Favorites] (
        [Id] int NOT NULL IDENTITY,
        [CustomerId] int NOT NULL,
        [ProductId] int NULL,
        [FarmerId] int NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Favorites] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Favorites_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Favorites_Users_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Favorites_Users_FarmerId] FOREIGN KEY ([FarmerId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [ProductId] int NOT NULL,
        [ProductName] nvarchar(120) NOT NULL,
        [Unit] nvarchar(20) NOT NULL,
        [UnitPrice] decimal(10,2) NOT NULL,
        [Quantity] int NOT NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE TABLE [Reviews] (
        [Id] int NOT NULL IDENTITY,
        [ProductId] int NOT NULL,
        [CustomerId] int NOT NULL,
        [FarmerId] int NOT NULL,
        [Rating] int NOT NULL,
        [Comment] nvarchar(1000) NULL,
        [ReviewDate] datetime2 NOT NULL,
        [FarmerResponse] nvarchar(600) NULL,
        [ResponseDate] datetime2 NULL,
        [IsVisible] bit NOT NULL,
        CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Reviews_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Reviews_Users_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Reviews_Users_FarmerId] FOREIGN KEY ([FarmerId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Favorites_CustomerId_FarmerId] ON [Favorites] ([CustomerId], [FarmerId]) WHERE [FarmerId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Favorites_CustomerId_ProductId] ON [Favorites] ([CustomerId], [ProductId]) WHERE [ProductId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Favorites_FarmerId] ON [Favorites] ([FarmerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Favorites_ProductId] ON [Favorites] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MarketFarmers_FarmerId] ON [MarketFarmers] ([FarmerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_MarketFarmers_MarketId_FarmerId] ON [MarketFarmers] ([MarketId], [FarmerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OrderItems_ProductId] ON [OrderItems] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_CustomerId] ON [Orders] ([CustomerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_FarmerId] ON [Orders] ([FarmerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Orders_OrderNumber] ON [Orders] ([OrderNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PickupSlots_FarmerId] ON [PickupSlots] ([FarmerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_FarmerId] ON [Products] ([FarmerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_Name_FarmerId] ON [Products] ([Name], [FarmerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reports_GeneratedBy] ON [Reports] ([GeneratedBy]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reviews_CustomerId] ON [Reviews] ([CustomerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reviews_FarmerId] ON [Reviews] ([FarmerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reviews_ProductId] ON [Reviews] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260927095043_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260927095043_InitialCreate', N'10.0.0');
END;

COMMIT;
GO

