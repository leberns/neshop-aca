using Microsoft.Extensions.Logging;

namespace Core.Products;

public partial class ProductsReader
{
    [LoggerMessage(LogLevel.Information, "{method} retrieved {productsCount} product(s)")]
    static partial void LogProductsRetrieved(ILogger<ProductsReader> logger, string method, int productsCount);

    [LoggerMessage(LogLevel.Information, "{Method} {ProductId}")]
    static partial void LogGetProductById(ILogger<ProductsReader> logger, string method, int productId);
}
