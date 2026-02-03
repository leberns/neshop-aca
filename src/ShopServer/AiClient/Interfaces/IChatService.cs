using Contracts.ProductsAiSearch.ApiModels;

namespace AiClient.Interfaces;

public interface IChatService
{
    public Task<QueryResponse> RespondAsync(string systemMessage, string userQuery, string assistantMessage, CancellationToken cancellationToken);
}
