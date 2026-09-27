
using MarketLink.Models;
using MarketLink.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketLink.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _auth;

    public AccountController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginVm
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = await _auth.LoginAsync(
            vm.Email,
            vm.Password
        );

        if (user == null)
        {
            ModelState.AddModelError(
                "",
                "Invalid email or password."
            );

            return View(vm);
        }

        HttpContext.Session.SetInt32(
            "UserId",
            user.Id
        );

        HttpContext.Session.SetString(
            "UserName",
            user.FullName
        );

        HttpContext.Session.SetString(
            "Role",
            user.Role.ToString()
        );

        if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) &&
            Url.IsLocalUrl(vm.ReturnUrl))
        {
            return Redirect(vm.ReturnUrl);
        }

        return user.Role switch
        {
            UserRole.Admin =>
                RedirectToAction("Dashboard", "Admin"),

            UserRole.Farmer =>
                RedirectToAction("Dashboard", "Farmer"),

            _ =>
                RedirectToAction("Dashboard", "Customer")
        };
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        try
        {
            var u = await _auth.RegisterAsync(
                vm.FullName,
                vm.Email,
                vm.Password,
                UserRole.Customer,
                vm.Phone,
                vm.Address
            );

            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            return View(vm);
        }
    }

    [HttpGet]
    public IActionResult FarmerRegister()
    {
        return View(new RegisterVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FarmerRegister(RegisterVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        try
        {
            await _auth.RegisterAsync(
                vm.FullName,
                vm.Email,
                vm.Password,
                UserRole.Farmer,
                vm.Phone,
                vm.Address
            );

            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            return View(vm);
        }
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }
}

public class LoginVm
{
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string Email { get; set; } = "";

    [System.ComponentModel.DataAnnotations.Required]
    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
}

public class RegisterVm
{
    [System.ComponentModel.DataAnnotations.Required]
    public string FullName { get; set; } = "";

    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string Email { get; set; } = "";

    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MinLength(6)]
    public string Password { get; set; } = "";

    public string? Phone { get; set; }

    public string? Address { get; set; }
} 
