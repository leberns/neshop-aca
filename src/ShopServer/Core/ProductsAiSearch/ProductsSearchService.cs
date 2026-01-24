using Contracts.Products.ApiModels;
using Contracts.ProductsAiSearch.Services;
using Core.Products;
using Microsoft.Extensions.Logging;

namespace Core.ProductsAiSearch;

public partial class ProductsSearchService(
    ILogger<ProductsSearchService> logger,
    IProductRagService productsRagService
    ) : IProductsSearchService
{
    public async Task<List<ProductInfo>> SearchProductsAsync(string userQuery, CancellationToken cancellationToken)
    {
        LogSearchingProductsForQueryQuery(logger, userQuery);

        var products = await productsRagService.SearchSimilarProductsAsync(userQuery, 5, cancellationToken);

        return products.Select(p => p.ToProductInfo()).ToList();
    }
}