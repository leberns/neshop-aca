using AiClient.Interfaces;
using Azure.AI.OpenAI;
using OpenAI.Embeddings;

namespace AiClient.AzureEmbedding;

/// <summary>
/// Embed text using Azure OpenAI Embeddings
/// </summary>
/// <param name="clientFactory">the Azure OpenAI client</param>
public class AzureTextEmbedder(
    AzureOpenAIClient clientFactory
    ) : ITextEmbedder
{
    public async Task<float[]> EmbedTextAsync(string text, CancellationToken cancellationToken)
    {
        var client = clientFactory.GetEmbeddingClient(Contracts.Constants.AiAzureEmbedding.ModelName);

        OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);

        return embedding.ToFloats().ToArray();
    }
}