namespace AiClient.Interfaces;

public interface IChatService
{
    public Task GenerateEmbeddingsAsync(CancellationToken cancellationToken);
}
