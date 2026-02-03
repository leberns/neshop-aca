using Contracts.Products.ApiModels;

namespace Contracts.Products.Services;

public interface IProductsAiSearchService
{
    Task<string> ProductsChatAsync(string userQuery, CancellationToken cancellationToken);
}