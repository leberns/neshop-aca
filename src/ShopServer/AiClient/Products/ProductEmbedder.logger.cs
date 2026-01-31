using Microsoft.Extensions.Logging;

namespace AiClient.Products;

public partial class ProductEmbedder
{
    [LoggerMessage(LogLevel.Information, "EmbedProduct {productId}")]
    static partial void LogEmbedProduct(ILogger<ProductEmbedder> logger, string productId);
}