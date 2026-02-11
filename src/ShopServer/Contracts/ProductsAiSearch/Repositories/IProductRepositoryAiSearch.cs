using Contracts.Products.Entities;
using Contracts.ProductsAiSearch.Entities;

namespace Contracts.ProductsAiSearch.Repositories;

public interface IProductRepositoryAiSearch
{
    Task<List<Product>> GetSearchableProducts(CancellationToken cancellationToken);

    Task<bool> AnyProductEmbeddings(CancellationToken cancellationToken);

    Task<List<Product>> SearchSimilarProducts(
        float[] vector,
        int limit,
        CancellationToken cancellationToken);

    Task<ProductEmbedding?> FindProductEmbeddingById(int productId, CancellationToken cancellationToken);

    Task AddProductEmbedding(ProductEmbedding productEmbedding, CancellationToken cancellationToken);

    Task UpdateProductEmbedding(ProductEmbedding productEmbedding, CancellationToken cancellationToken);
}