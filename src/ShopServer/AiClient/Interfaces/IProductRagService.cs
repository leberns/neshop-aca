using Contracts.ProductsAiSearch.Models;

namespace AiClient.Interfaces;

public interface IProductRagService
{
    /// <summary>
    /// Answer user questions about the products available.
    /// </summary>
    /// <param name="userQuery">query, ex.: "are there any small tents on sale?"</param>
    /// <param name="cancellationToken"></param>
    Task<RagResponse> RespondAsync(
        string userQuery,
        CancellationToken cancellationToken);
}
