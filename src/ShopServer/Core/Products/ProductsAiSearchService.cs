using AiClient.Interfaces;
using Contracts.Products.Services;
using Microsoft.Extensions.Logging;

namespace Core.Products;

public partial class ProductsAiSearchService(
    ILogger<ProductsAiSearchService> logger,
    IProductRagService productsRagService
    ) : IProductsAiSearchService
{
    public async Task<string> ProductsChatAsync(string userQuery, CancellationToken cancellationToken)
    {
        LogProductsChat(logger, userQuery);

        return await productsRagService.RespondAsync(userQuery, cancellationToken);
    }
}