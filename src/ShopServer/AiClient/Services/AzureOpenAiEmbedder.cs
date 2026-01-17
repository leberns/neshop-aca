using Azure.AI.OpenAI;
using Azure.Identity;
using Contracts.Options;
using Contracts.Services;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;

namespace AiClient.Services;

public class AzureOpenAiEmbedder(
    IOptions<AiOptions> aiOptions,
    IOptions<ManagedIdentityOptions> managedIdentityOptions
    ) : IEmbedder
{
    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken)
    {
        var credential = new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = managedIdentityOptions.Value.ManagedIdentityClientId
            });

        AzureOpenAIClient openAiClient = new(new Uri(aiOptions.Value.Endpoint), credential);

        var client = openAiClient.GetEmbeddingClient(aiOptions.Value.EmbeddingModel);
        OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);
        return embedding.ToFloats().ToArray();
    }
}