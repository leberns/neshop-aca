using Contracts.Products.ApiModels;

namespace Contracts.ProductsAiSearch.Services;

public interface IProductsSearchService
{
    Task<List<ProductInfo>> SearchProductsAsync(string userQuery, CancellationToken cancellationToken);
}