using Microsoft.Extensions.Logging;

namespace Core.Products;

public partial class ProductsSearchService
{
    [LoggerMessage(LogLevel.Information, "LogSearchProducts {userQuery}")]
    static partial void LogSearchProducts(ILogger<ProductsSearchService> logger, string userQuery);
}