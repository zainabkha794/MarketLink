# MarketLink — Complete .NET MVC Marketplace

This is a merged, single-project ASP.NET Core MVC application for a farmer-to-customer marketplace.

## Stack
- ASP.NET Core MVC / .NET 10
- C#
- Entity Framework Core 10
- SQLite
- Razor Views
- Session-based login for the demo
- REST endpoints under `/api`

## Project structure

```text
MarketLink/
├── Controllers/       # MVC + REST controllers
├── Data/              # EF Core DbContext + seed data
├── Dtos/              # API request DTOs
├── Models/            # Database entities and view models
├── Services/          # Auth, products and orders business logic
├── Views/              # Razor UI
├── wwwroot/            # CSS/JS/static assets
├── appsettings.json
├── Program.cs
└── MarketLink.csproj
```

## Main features
- Customer registration/login
- Farmer registration/login
- Admin demo login
- Product marketplace with search/category filtering
- Product details and farmer profile
- Shopping cart
- Checkout and order creation
- Customer order dashboard
- Farmer product management and related orders
- Admin order-status management
- SQLite database with automatic first-run creation and seed data
- REST API for authentication, products and orders
- Small help/chatbot page

## Demo accounts

```text
Admin:
admin@marketlink.local
Admin@123

Farmer:
farmer@marketlink.local
Farmer@123

Customer:
customer@marketlink.local
Customer@123
```

## Run

Install the .NET 10 SDK, then from this folder:

```bash
dotnet restore
dotnet run
```

Open the HTTPS URL printed by the terminal.

The SQLite database `marketlink.db` is created automatically on first run.

## API examples

```text
GET  /api/products
GET  /api/products/1
POST /api/auth/login
POST /api/auth/register
POST /api/orders
```

For a production deployment, replace the demo SHA-256 password storage with ASP.NET Core Identity/password hashing, add CSRF/authentication policies to APIs, use secrets management, and configure a production database.
