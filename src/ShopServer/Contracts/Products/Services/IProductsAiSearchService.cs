using Contracts.ProductsAiSearch.ApiModels;

namespace Contracts.Products.Services;

public interface IProductsAiSearchService
{
    Task<QueryResponse> ProductsChatAsync(string userQuery, CancellationToken cancellationToken);
}