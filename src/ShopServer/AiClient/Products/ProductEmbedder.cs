using Pgvector;
using AiClient.Interfaces;
using Contracts.Products.Entities;
using Contracts.ProductsAiSearch.Entity;
using Contracts.ProductsAiSearch.Repositories;

namespace AiClient.Products;

public partial class ProductEmbedder(
    ITextEmbedder textEmbedder,
    IProductRepositoryAiSearch productAiSearch
    ) : IProductEmbedder
{
    public async Task GenerateEmbeddingsAsync(CancellationToken cancellationToken)
    {
        var products = await productAiSearch.GetSearchableProducts(cancellationToken);

        foreach (var product in products)
        {
            await EmbedProductAsync(product, cancellationToken);
        }
    }

    private async Task EmbedProductAsync(Product product, CancellationToken cancellationToken)
    {
        var content = product.ToEmbeddingContent();

        //var embedding = await textEmbedder.GenerateEmbeddingAsync(content, cancellationToken);
        var embedding = new float[1536];
        for (var i = 0; i < embedding.Length; i++)
        {
            embedding[i] = 64;
        }

        var productEmbedding = await productAiSearch.FindProductEmbeddingById(product.Id, cancellationToken);

        if (productEmbedding is null)
        {
            var newProductEmbedding = new ProductEmbedding
            {
                Id = 0,
                ProductId = product.Id,
                Product = product,
                Price = product.Price,
                Category = product.Category.Name,
                Brand = product.Brand.Name,
                Content = content,
                Embedding = new Vector(embedding),
                GeneratedAtUtc = DateTime.UtcNow,
                Deployment = Contracts.Constants.AiAzureEmbedding.DeploymentName,
            };

            await productAiSearch.AddProductEmbedding(newProductEmbedding, cancellationToken);
        }
        else
        {
            var updatedProductEmbedding = productEmbedding with
            {
                Price = product.Price,
                Category = product.Category.Name,
                Brand = product.Brand.Name,
                Content = content,
                Embedding = new Vector(embedding),
                GeneratedAtUtc = DateTime.UtcNow,
                Deployment = Contracts.Constants.AiAzureEmbedding.DeploymentName
            };

            await productAiSearch.UpdateProductEmbedding(updatedProductEmbedding, cancellationToken);
        }
    }
}
