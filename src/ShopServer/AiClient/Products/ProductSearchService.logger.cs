using Microsoft.Extensions.Logging;

namespace AiClient.Products;

public partial class ProductSearchService
{
    [LoggerMessage(LogLevel.Information, "SearchingProductsWithUserQuery {userQuery}")]
    static partial void LogSearchingProductsWithUserQuery(ILogger<ProductSearchService> logger, string userQuery);}