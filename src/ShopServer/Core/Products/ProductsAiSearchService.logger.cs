using Microsoft.Extensions.Logging;

namespace Core.Products;

public partial class ProductsAiSearchService
{
    [LoggerMessage(LogLevel.Information, "ProductsChat {userQuery}")]
    static partial void LogProductsChat(ILogger<ProductsAiSearchService> logger, string userQuery);
}