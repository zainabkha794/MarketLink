using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class FarmerController : Controller
{
    private readonly MarketLinkContext _db;
    private readonly IOrderService _orders;
    private readonly IWebHostEnvironment _env;

    public FarmerController(
        MarketLinkContext db,
        IOrderService orders,
        IWebHostEnvironment env)
    {
        _db = db;
        _orders = orders;
        _env = env;
    }

    private int? Id => HttpContext.Session.GetInt32("UserId");

    public async Task<IActionResult> Dashboard()
    {
        if (Id == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(new FarmerDashVm
        {
            Products = await _db.Products
                .Where(x => x.FarmerId == Id)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(),

            Orders = await _orders.ForFarmerAsync(Id.Value)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProduct(Product p, IFormFile? ImageFile)
    {
        if (Id == null)
        {
            return RedirectToAction("Login", "Account");
        }

        p.Id = 0;
        p.FarmerId = Id.Value;
        p.IsActive = true;
        p.CreatedAt = DateTime.UtcNow;

        if (ImageFile != null && ImageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }

            p.ImageUrl = "/uploads/" + uniqueFileName;
        }

        _db.Products.Add(p);
        await _db.SaveChangesAsync();

        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleProductStatus(int id)
    {
        if (Id == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var product = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == id && x.FarmerId == Id);

        if (product != null)
        {
            product.IsActive = !product.IsActive;
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (Id == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var product = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == id && x.FarmerId == Id);

        if (product != null)
        {
            if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl.StartsWith("/uploads/"))
            {
                var relativePath = product.ImageUrl.TrimStart('/');
                var fullPath = Path.Combine(_env.WebRootPath, relativePath);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Dashboard");
    }
}

public class FarmerDashVm
{
    public List<Product> Products { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}