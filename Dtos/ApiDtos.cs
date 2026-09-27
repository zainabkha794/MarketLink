using System.ComponentModel.DataAnnotations;

namespace MarketLink.Dtos;

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record RegisterDto(
    [Required] string FullName,
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password,
    string? Phone,
    string? Address
);

public record CreateProductDto(
    [Required] string Name,
    [Required] string Category,
    [Range(0, 10000000)] decimal Price,
    [Range(0, 1000000)] int StockQuantity,
    string? Description,
    string? Unit
);

public record CreateOrderDto(
    [Required] List<OrderLineDto> Items,
    [Required] string DeliveryAddress
);

public record OrderLineDto(
    int ProductId,
    int Quantity
);