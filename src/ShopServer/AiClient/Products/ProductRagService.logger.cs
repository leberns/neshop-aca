using Microsoft.Extensions.Logging;

namespace AiClient.Products;

public partial class ProductRagService
{
    [LoggerMessage(LogLevel.Information, "ProductsChatResponding {userQuery}")]
    static partial void LogProductsChatResponding(ILogger<ProductRagService> logger, string userQuery);

    [LoggerMessage(LogLevel.Information, "FoundRelevantProducts {count}, {products}")]
    static partial void LogFoundRelevantProducts(ILogger<ProductRagService> logger, int count, string products);

    [LoggerMessage(LogLevel.Information, "GeneratedResponseForUserQuery {response}")]
    static partial void LogGeneratedResponseForUserQuery(ILogger<ProductRagService> logger, string response);
}