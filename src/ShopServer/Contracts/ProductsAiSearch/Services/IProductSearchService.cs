using Contracts.Products.Entities;

namespace Contracts.ProductsAiSearch.Services;

public interface IProductSearchService
{
    /// <summary>
    /// Search for products based on the user query.
    /// </summary>
    /// <param name="userQuery">query, ex.: "what small tent is there?"</param>
    /// <param name="limit">how many embedded items to return (top-k "chunks")</param>
    /// <param name="cancellationToken"></param>
    Task<List<Product>> SearchSimilarProductsAsync(
        string userQuery,
        int limit,
        CancellationToken cancellationToken);
}
