using Contracts.Products.ApiModels;
using Contracts.Products.Services;
using Contracts.ProductsAiSearch.Services;
using Microsoft.Extensions.Logging;

namespace Core.Products;

public partial class ProductsSearchService(
    ILogger<ProductsSearchService> logger,
    IProductSearchService productsSearchService
    ) : IProductsSearchService
{
    public async Task<List<ProductInfo>> SearchProductsAsync(string userQuery, CancellationToken cancellationToken)
    {
        LogSearchProducts(logger, userQuery);

        var products = await productsSearchService.SearchSimilarProductsAsync(userQuery, 5, cancellationToken);

        return products.Select(p => p.ToProductInfo()).ToList();
    }
}