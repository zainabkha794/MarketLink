using MarketLink.Data;
using MarketLink.Models;
using MarketLink.Models.ViewModels;
using MarketLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _auth;
    private readonly MarketLinkContext _db;

    public AccountController(IAuthService auth, MarketLinkContext db)
    {
        _auth = auth;
        _db = db;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null) =>
        View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _auth.FindByEmailAsync(model.Email);
        if (user == null || !PasswordHasher.Verify(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }
        if (!user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "This account has been deactivated. Please contact the platform office.");
            return View(model);
        }

        await _auth.SignInAsync(HttpContext, user, model.RememberMe);

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return user.Role switch
        {
            UserRole.Admin => RedirectToAction("Dashboard", "Admin"),
            UserRole.Farmer => RedirectToAction("Dashboard", "Farmer"),
            _ => RedirectToAction("Dashboard", "Customer")
        };
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _auth.FindByEmailAsync(model.Email) != null)
        {
            ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
            return View(model);
        }

        var user = new User
        {
            FullName = model.FullName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            PasswordHash = PasswordHasher.Hash(model.Password),
            Role = UserRole.Customer,
            IsApproved = true
        };
        await _auth.CreateAsync(user);
        await _auth.SignInAsync(HttpContext, user, true);
        TempData["Success"] = "Welcome to MarketLink! Your market basket awaits.";
        return RedirectToAction("Dashboard", "Customer");
    }

    [HttpGet]
    public async Task<IActionResult> RegisterFarmer()
    {
        ViewBag.Markets = await _db.Markets.Where(m => m.IsActive).ToListAsync();
        return View(new RegisterFarmerViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterFarmer(RegisterFarmerViewModel model)
    {
        ViewBag.Markets = await _db.Markets.Where(m => m.IsActive).ToListAsync();
        if (!ModelState.IsValid) return View(model);
        if (await _auth.FindByEmailAsync(model.Email) != null)
        {
            ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
            return View(model);
        }

        var user = new User
        {
            FullName = model.FullName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            PasswordHash = PasswordHasher.Hash(model.Password),
            Role = UserRole.Farmer,
            StallName = model.StallName,
            Bio = model.Bio,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            IsApproved = false // admin approval required before listing
        };
        await _auth.CreateAsync(user);

        foreach (var marketId in model.MarketIds.Distinct())
            _db.MarketFarmers.Add(new MarketFarmer { FarmerId = user.Id, MarketId = marketId });
        await _db.SaveChangesAsync();

        TempData["Info"] = "Your stall application has been submitted. An admin will review it shortly — you can browse the market in the meantime.";
        return RedirectToAction("Login");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _auth.SignOutAsync(HttpContext);
        TempData["Success"] = "You have signed out. See you at the market!";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _auth.CurrentUser(User);
        if (user == null) return RedirectToAction("Login");

        ViewBag.Markets = await _db.Markets.Where(m => m.IsActive).ToListAsync();
        ViewBag.CurrentMarketIds = await _db.MarketFarmers
            .Where(mf => mf.FarmerId == user.Id)
            .Select(mf => mf.MarketId)
            .ToListAsync();

        return View(new ProfileViewModel
        {
            FullName = user.FullName,
            Phone = user.Phone,
            Address = user.Address,
            StallName = user.StallName,
            Bio = user.Bio,
            ProfileImageUrl = user.ProfileImageUrl,
            Latitude = user.Latitude,
            Longitude = user.Longitude,
            OrderCutoffHours = user.OrderCutoffHours,
            MarketIds = ViewBag.CurrentMarketIds as List<int> ?? new List<int>()
        });
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        var user = await _auth.CurrentUser(User);
        if (user == null) return RedirectToAction("Login");
        ViewBag.Markets = await _db.Markets.Where(m => m.IsActive).ToListAsync();

        if (!ModelState.IsValid) return View(model);

        user.FullName = model.FullName;
        user.Phone = model.Phone;
        user.Address = model.Address;
        user.ProfileImageUrl = model.ProfileImageUrl;
        if (user.Role == UserRole.Farmer)
        {
            user.StallName = model.StallName;
            user.Bio = model.Bio;
            user.Latitude = model.Latitude;
            user.Longitude = model.Longitude;
            user.OrderCutoffHours = model.OrderCutoffHours;

            var existing = await _db.MarketFarmers.Where(mf => mf.FarmerId == user.Id).ToListAsync();
            _db.MarketFarmers.RemoveRange(existing);
            foreach (var marketId in model.MarketIds.Distinct())
                _db.MarketFarmers.Add(new MarketFarmer { FarmerId = user.Id, MarketId = marketId });
        }
        await _db.SaveChangesAsync();
        TempData["Success"] = "Profile updated.";
        return RedirectToAction("Profile");
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var user = await _auth.CurrentUser(User);
        if (user == null) return RedirectToAction("Login");

        if (!PasswordHasher.Verify(currentPassword, user.PasswordHash))
            TempData["Error"] = "Your current password is incorrect.";
        else if (newPassword.Length < 6)
            TempData["Error"] = "The new password must be at least 6 characters.";
        else if (newPassword != confirmPassword)
            TempData["Error"] = "The new passwords do not match.";
        else
        {
            user.PasswordHash = PasswordHasher.Hash(newPassword);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Password changed.";
        }
        return RedirectToAction("Profile");
    }
}
