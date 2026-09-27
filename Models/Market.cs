using System.ComponentModel.DataAnnotations;

namespace MarketLink.Models;

public class Market
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = "";

    [MaxLength(250)]
    public string Location { get; set; } = "";

    [MaxLength(500)]
    public string Description { get; set; } = "";
}