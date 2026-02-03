using Microsoft.Extensions.Logging;
using AiClient.Interfaces;
using Contracts.Products.Services;
using Contracts.ProductsAiSearch.ApiModels;

namespace Core.Products;

public partial class ProductsAiSearchService(
    ILogger<ProductsAiSearchService> logger,
    IProductRagService productsRagService
    ) : IProductsAiSearchService
{
    public async Task<QueryResponse> ProductsChatAsync(string userQuery, CancellationToken cancellationToken)
    {
        LogProductsChat(logger, userQuery);

        return await productsRagService.RespondAsync(userQuery, cancellationToken);
    }
}