using Microsoft.Extensions.Logging;

namespace Core.ProductsAiSearch;

public partial class ProductsSearchService
{
    [LoggerMessage(LogLevel.Information, "Searching products with user query: {query}")]
    static partial void LogSearchingProductsForQueryQuery(ILogger<ProductsSearchService> logger, string query);
}