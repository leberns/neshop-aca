using Contracts;
using Contracts.Embedding.Services;
using Contracts.Products.Entities;
using Contracts.ProductsAiSearch.Entity;
using Contracts.ProductsAiSearch.Repositories;
using Contracts.ProductsAiSearch.Services;

namespace Core.ProductsAiSearch;

public class ProductRagService(
    IEmbedder embedder,
    IProductRepositoryAiSearch productAiSearch
    ) : IProductRagService
{
    public async Task GenerateAndStoreEmbeddingsAsync(CancellationToken cancellationToken)
    {
        var products = await productAiSearch.GetSearchableProducts(cancellationToken);

        foreach (var product in products)
        {
            await EmbedProductAsync(product, cancellationToken);
        }
    }

    private async Task EmbedProductAsync(Product product, CancellationToken cancellationToken)
    {
        var sourceText = $"{product.Name} {product.Description}";
        var embedding = await embedder.GenerateEmbeddingAsync(sourceText, cancellationToken);

        var productEmbedding = await productAiSearch.FindProductEmbeddingById(product.Id, cancellationToken);

        if (productEmbedding is null)
        {
            var newProductEmbedding = new ProductEmbedding
            {
                ProductId = product.Id,
                SourceText = sourceText,
                Vector = embedding,
                GeneratedAtUtc = DateTime.UtcNow,
                Model = Constants.Ai.EmbeddingModelName
            };

            await productAiSearch.AddProductEmbedding(newProductEmbedding, cancellationToken);
        }
        else
        {
            var updatedProductEmbedding = productEmbedding with
            {
                SourceText = sourceText,
                Vector = embedding,
                GeneratedAtUtc = DateTime.UtcNow,
                Model = Constants.Ai.EmbeddingModelName
            };

            await productAiSearch.UpdateProductEmbedding(updatedProductEmbedding, cancellationToken);
        }
    }

    public async Task<List<Product>> SearchSimilarProductsAsync(
        string query,
        int limit,
        CancellationToken cancellationToken)
    {
        if (!await productAiSearch.AnyProductEmbeddings(cancellationToken))
        {
            await GenerateAndStoreEmbeddingsAsync(cancellationToken);
        }

        var queryEmbedding = await embedder.GenerateEmbeddingAsync(query, cancellationToken);

        return await productAiSearch.SearchSimilarProducts(queryEmbedding, limit, cancellationToken);
    }
}
