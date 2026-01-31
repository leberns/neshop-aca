using Contracts.Products.ApiModels;

namespace Contracts.Products.Services;

public interface IProductsSearchService
{
    Task<List<ProductInfo>> SearchProductsAsync(string userQuery, CancellationToken cancellationToken);
}