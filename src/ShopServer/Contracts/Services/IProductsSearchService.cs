using Contracts.ApiModels;

namespace Contracts.Services;

public interface IProductsSearchService
{
    Task<List<ProductInfo>> SearchProductsAsync(string userQuery, CancellationToken cancellationToken);
}