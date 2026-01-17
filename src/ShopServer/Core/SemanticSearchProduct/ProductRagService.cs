using Contracts.DataModels;
using Contracts.Options;
using Contracts.Services;
using Database;
using Microsoft.Extensions.Options;
using Pgvector;

namespace Core.SemanticSearchProduct;

public class ProductRagService(
    IEmbedder embedder,
    ShopRepository repository,
    IOptions<AiOptions> aiOptions
    ) : IProductRagService
{
    public async Task GenerateAndStoreEmbeddingsAsync(CancellationToken cancellationToken)
    {
        var products = await repository.GetProducts(cancellationToken);

        foreach (var product in products)
        {
            await EmbedProductAsync(product, cancellationToken);
        }
    }

    private async Task EmbedProductAsync(Product product, CancellationToken cancellationToken)
    {
        var sourceText = $"{product.Name} {product.Description}";
        var embedding = await embedder.GenerateEmbeddingAsync(sourceText, cancellationToken);

        var productEmbedding = await repository.FindProductEmbeddingById(product.Id, cancellationToken);

        if (productEmbedding is null)
        {
            productEmbedding = new ProductEmbedding
            {
                ProductId = product.Id,
                SourceText = sourceText,
                Vector = new Vector(embedding),
                GeneratedAtUtc = DateTime.UtcNow,
                Model = aiOptions.Value.EmbeddingModel
            };

            await repository.AddProductEmbedding(productEmbedding, cancellationToken);
        }
        else
        {
            productEmbedding.SourceText = sourceText;
            productEmbedding.Vector = new Vector(embedding);
            productEmbedding.GeneratedAtUtc = DateTime.UtcNow;
            productEmbedding.Model = aiOptions.Value.EmbeddingModel;

            await repository.UpdateProductEmbedding(productEmbedding, cancellationToken);
        }
    }

    public async Task<List<Product>> SearchSimilarProductsAsync(
        string query,
        int limit,
        CancellationToken cancellationToken)
    {
        var queryEmbedding = await embedder.GenerateEmbeddingAsync(query, cancellationToken);

        var queryVector = new Vector(queryEmbedding);

        return await repository.SearchSimilarProducts(queryVector, limit, cancellationToken);
    }
}
