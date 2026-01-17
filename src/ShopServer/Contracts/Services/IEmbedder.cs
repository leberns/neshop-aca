namespace Contracts.Services;

public interface IEmbedder
{
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken);
}