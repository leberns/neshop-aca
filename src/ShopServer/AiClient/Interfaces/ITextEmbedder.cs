namespace AiClient.Interfaces;

public interface ITextEmbedder
{
    Task<float[]> EmbedTextAsync(string text, CancellationToken cancellationToken);
}