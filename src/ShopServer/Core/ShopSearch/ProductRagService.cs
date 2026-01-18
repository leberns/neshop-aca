using Contracts;
using Contracts.DataModels;
using Contracts.Services;
using Database;

namespace Core.ShopSearch;

public class ProductRagService(
    IEmbedder embedder,
    ShopProductSearch shopProductSearch
    ) : IProductRagService
{
    public async Task GenerateAndStoreEmbeddingsAsync(CancellationToken cancellationToken)
    {
        var products = await shopProductSearch.GetSearchableProducts(cancellationToken);

        foreach (var product in products)
        {
            await EmbedProductAsync(product, cancellationToken);
        }
    }

    private async Task EmbedProductAsync(Product product, CancellationToken cancellationToken)
    {
        var sourceText = $"{product.Name} {product.Description}";
        var embedding = await embedder.GenerateEmbeddingAsync(sourceText, cancellationToken);

        var productEmbedding = await shopProductSearch.FindProductEmbeddingById(product.Id, cancellationToken);

        if (productEmbedding is null)
        {
            productEmbedding = new ProductEmbedding
            {
                ProductId = product.Id,
                SourceText = sourceText,
                Vector = embedding,
                GeneratedAtUtc = DateTime.UtcNow,
                Model = Constants.Ai.EmbeddingModelName
            };

            await shopProductSearch.AddProductEmbedding(productEmbedding, cancellationToken);
        }
        else
        {
            productEmbedding.SourceText = sourceText;
            productEmbedding.Vector = embedding;
            productEmbedding.GeneratedAtUtc = DateTime.UtcNow;
            productEmbedding.Model = Constants.Ai.EmbeddingModelName;

            await shopProductSearch.UpdateProductEmbedding(productEmbedding, cancellationToken);
        }
    }

    public async Task<List<Product>> SearchSimilarProductsAsync(
        string query,
        int limit,
        CancellationToken cancellationToken)
    {
        if (!await shopProductSearch.AnyProductEmbeddings(cancellationToken))
        {
            await GenerateAndStoreEmbeddingsAsync(cancellationToken);
        }

        var queryEmbedding = await embedder.GenerateEmbeddingAsync(query, cancellationToken);

        return await shopProductSearch.SearchSimilarProducts(queryEmbedding, limit, cancellationToken);
    }
}
