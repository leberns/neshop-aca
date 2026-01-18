using System.ComponentModel.DataAnnotations;

namespace Contracts.Options;

public record AiOptions
{
    [Required]
    public string Endpoint { get; set; } = string.Empty;

    public string? ApiKey { get; set; }
}
