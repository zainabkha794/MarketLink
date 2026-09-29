using System.Security.Claims;
using MarketLink.Data;
using MarketLink.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Services;

public interface IAuthService
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task SignInAsync(HttpContext http, User user, bool persistent);
    Task SignOutAsync(HttpContext http);
    int? CurrentUserId(ClaimsPrincipal? principal);
    UserRole? CurrentRole(ClaimsPrincipal? principal);
    Task<User?> CurrentUser(ClaimsPrincipal? principal);
}

public class AuthService : IAuthService
{
    private readonly MarketLinkContext _db;
    public AuthService(MarketLinkContext db) => _db = db;

    public async Task<User?> FindByEmailAsync(string email) =>
        await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task<User?> FindByIdAsync(int id) => await _db.Users.FindAsync(id);

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task SignInAsync(HttpContext http, User user, bool persistent)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await http.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = persistent, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14) });
    }

    public async Task SignOutAsync(HttpContext http) =>
        await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    public int? CurrentUserId(ClaimsPrincipal? principal)
    {
        var raw = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }

    public UserRole? CurrentRole(ClaimsPrincipal? principal)
    {
        var raw = principal?.FindFirstValue(ClaimTypes.Role);
        return Enum.TryParse<UserRole>(raw, out var role) ? role : null;
    }

    public async Task<User?> CurrentUser(ClaimsPrincipal? principal)
    {
        var id = CurrentUserId(principal);
        return id == null ? null : await _db.Users.FindAsync(id);
    }
}
