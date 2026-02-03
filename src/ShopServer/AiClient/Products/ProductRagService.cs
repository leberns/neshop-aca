using System.Text;
using AiClient.Interfaces;
using Contracts.Products.Entities;
using Contracts.ProductsAiSearch.ApiModels;
using Contracts.ProductsAiSearch.Repositories;
using Microsoft.Extensions.Logging;

namespace AiClient.Products;

public partial class ProductRagService(
    IProductRepositoryAiSearch repositoryAiSearch,
    IProductEmbedder productEmbedder,
    ITextEmbedder textEmbedder,
    IChatService chatService,
    ILogger<ProductRagService> logger
    ) : IProductRagService
{
    public async Task<QueryResponse> RespondAsync(
        string userQuery,
        CancellationToken cancellationToken)
    {
        LogSearchingProductsWithUserQuery(logger, userQuery);

        if (!await repositoryAiSearch.AnyProductEmbeddings(cancellationToken))
        {
            await productEmbedder.GenerateEmbeddingsAsync(cancellationToken);
        }

        const int limit = 5;

        var queryEmbedding = await textEmbedder.GenerateEmbeddingAsync(userQuery, cancellationToken);

        var relevantProducts = await repositoryAiSearch.SearchSimilarProducts(queryEmbedding, limit, cancellationToken);

        LogFoundRelevantProducts(logger, relevantProducts.Count, string.Join(", ", relevantProducts.Select(p => $"{p.Id} {p.Name}")));

        var systemMessage = BuildSystemMessage();

        var assistantMessage = BuildAssistantMessage(relevantProducts);

        var response = await chatService.RespondAsync(systemMessage, userQuery, assistantMessage, cancellationToken);

        LogGeneratedResponseForUserQuery(logger, response.Text);

        return response;
    }

    private static string BuildSystemMessage()
    {
        return """
               You are a helpful outdoor gear shopping assistant for the e-commerce store NeShop. 
               Answer the customer's question based ONLY on the provided products.
               Provide a helpful, concise answer based on the provided products.
               If recommending products, mention specific names and prices.
               If the question cannot be answered with the available information, say 'I don't know.'
               """;
    }

    private static string BuildAssistantMessage(
        List<Product> relevantProducts)
    {
        var assistantMessage = new StringBuilder();

        if (relevantProducts.Count == 0)
        {
            assistantMessage.AppendLine("No products provided.");
            return assistantMessage.ToString();
        }

        assistantMessage.AppendLine("Provided products:");

        foreach (var product in relevantProducts)
        {
            assistantMessage.Append(product.ToAssistantContent());
            assistantMessage.AppendLine();
        }

        return assistantMessage.ToString();
    }
}
