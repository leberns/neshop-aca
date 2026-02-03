using Microsoft.Extensions.Logging;

namespace AiClient.Products;

public partial class ProductRagService
{
    [LoggerMessage(LogLevel.Information, "SearchingProductsWithUserQuery {userQuery}")]
    static partial void LogSearchingProductsWithUserQuery(ILogger<ProductRagService> logger, string userQuery);}