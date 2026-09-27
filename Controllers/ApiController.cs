using MarketLink.Data;
using MarketLink.Dtos;
using MarketLink.Models;
using MarketLink.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketLink.Controllers;


[ApiController]
[Route("api/products")]
public class ProductsApiController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsApiController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? search,
        [FromQuery] string? category)
    {
        return Ok(await _service.GetAsync(search, category));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _service.GetByIdAsync(id);

        return p == null
            ? NotFound()
            : Ok(p);
    }
}


[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthApiController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var u = await _auth.LoginAsync(
            dto.Email,
            dto.Password
        );

        return u == null
            ? Unauthorized(new
            {
                message = "Invalid credentials"
            })
            : Ok(new
            {
                id = u.Id,
                u.FullName,
                u.Email,
                role = u.Role.ToString()
            });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var u = await _auth.RegisterAsync(
                dto.FullName,
                dto.Email,
                dto.Password,
                UserRole.Customer,
                dto.Phone,
                dto.Address
            );

            return Created(
                "",
                new
                {
                    id = u.Id,
                    u.FullName,
                    u.Email
                }
            );
        }
        catch (Exception ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }
}


[ApiController]
[Route("api/orders")]
public class OrdersApiController : ControllerBase
{
    private readonly IOrderService _orders;

    public OrdersApiController(IOrderService orders)
    {
        _orders = orders;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var id = HttpContext.Session.GetInt32("UserId");

        if (id == null)
        {
            return Unauthorized();
        }

        try
        {
            var o = await _orders.CreateAsync(
                id.Value,
                dto.Items
                    .Select(x => (x.ProductId, x.Quantity))
                    .ToList(),
                dto.DeliveryAddress
            );

            return Ok(new
            {
                id = o.Id,
                o.TotalAmount,
                status = o.Status.ToString()
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}

