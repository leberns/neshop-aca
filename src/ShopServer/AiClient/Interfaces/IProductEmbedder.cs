namespace AiClient.Interfaces;

public interface IProductEmbedder
{
    /// <summary>
    /// Generate embeddings for all products and store them in the database.
    /// </summary>
    public Task GenerateEmbeddingsAsync(CancellationToken cancellationToken);
}
