namespace Contracts.Embedding.Services;

public interface IEmbedder
{
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken);
}