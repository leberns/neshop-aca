using Contracts.ProductsAiSearch.ApiModels;

namespace Contracts.Products.Services;

public interface IProductsAiSearchService
{
    Task<UserSearchResponse> ProductsSearchAsync(string userQuery, CancellationToken cancellationToken);
}