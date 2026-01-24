using Azure.AI.OpenAI;
using Contracts;
using Contracts.Embedding.Services;
using OpenAI.Embeddings;

namespace AiClient.Services;

public class AppAzureOpenAiEmbedder(
    AzureOpenAIClient openAiClient
    ) : IEmbedder
{
    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken)
    {
        var client = openAiClient.GetEmbeddingClient(Constants.Ai.EmbeddingModelName);
        OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);
        return embedding.ToFloats().ToArray();
    }
}