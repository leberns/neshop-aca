using AiClient.Interfaces;
using Contracts.Products.Entities;
using Contracts.ProductsAiSearch.Repositories;
using Contracts.ProductsAiSearch.Services;

namespace AiClient.Products;

public partial class ProductSearchService(
    IProductRepositoryAiSearch repositoryAiSearch,
    IProductEmbedder productEmbedder,
    ITextEmbedder textEmbedder
    ) : IProductSearchService
{
    public async Task<List<Product>> SearchSimilarProductsAsync(
        string userQuery,
        int limit,
        CancellationToken cancellationToken)
    {
        if (!await repositoryAiSearch.AnyProductEmbeddings(cancellationToken))
        {
            await productEmbedder.GenerateEmbeddingsAsync(cancellationToken);
        }

        var queryEmbedding = await textEmbedder.EmbedTextAsync(userQuery, cancellationToken);

        return await repositoryAiSearch.SearchSimilarProducts(queryEmbedding, limit, cancellationToken);
    }
}
