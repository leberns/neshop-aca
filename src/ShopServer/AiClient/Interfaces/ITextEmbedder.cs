namespace AiClient.Interfaces;

public interface ITextEmbedder
{
    /// <summary>
    /// Generate the embedding for the given text.
    /// </summary>
    /// <param name="text">the text to embed</param>
    /// <param name="cancellationToken"></param>
    /// <returns>the embedding (vector representation of the given text)</returns>
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken);
}