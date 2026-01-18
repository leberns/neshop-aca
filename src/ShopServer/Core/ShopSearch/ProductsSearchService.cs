using Contracts.ApiModels;
using Contracts.Services;
using Core.ReadProduct;

namespace Core.ShopSearch;

public class ProductsSearchService(
    IProductRagService productsRagService
    ) : IProductsSearchService
{
    public async Task<List<ProductInfo>> SearchProductsAsync(string userQuery, CancellationToken cancellationToken)
    {
        var products = await productsRagService.SearchSimilarProductsAsync(userQuery, 5, cancellationToken);

        return products.Select(p => p.ToProductInfo()).ToList();
    }
}