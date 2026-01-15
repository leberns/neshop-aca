using System.ComponentModel.DataAnnotations;

namespace WebContracts.Options;

public record ShopClientOptions
{
    [Url]
    public required string BaseUrl { get; set; } = string.Empty;
}