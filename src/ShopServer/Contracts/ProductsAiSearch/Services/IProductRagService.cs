using Contracts.Products.Entities;

namespace Contracts.ProductsAiSearch.Services;

public interface IProductRagService
{
    /// <summary>
    /// Generate embeddings for all products and store them in the database.
    /// </summary>
    Task GenerateAndStoreEmbeddingsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// search for products based on the user query.
    /// </summary>
    /// <param name="query">user query, ex.: "what small tent do you offer?"</param>
    /// <param name="limit">how many embedded items to return (top-k "chunks")</param>
    /// <param name="cancellationToken"></param>
    Task<List<Product>> SearchSimilarProductsAsync(
        string query,
        int limit,
        CancellationToken cancellationToken);
}
