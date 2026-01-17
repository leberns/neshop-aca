using System.ComponentModel.DataAnnotations;

namespace Contracts.Options;

public record AiOptions
{
    [Required]
    public string Endpoint { get; set; } = string.Empty;

    [Required]
    public string EmbeddingModel { get; set; } = Constants.AiModels.DefaultEmbeddingModel;
}
