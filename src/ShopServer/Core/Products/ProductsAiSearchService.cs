using AiClient.Interfaces;
using Contracts.Products.Services;
using Contracts.ProductsAiSearch.ApiModels;

namespace Core.Products;

public partial class ProductsAiSearchService(
    IProductRagService productsRagService
    ) : IProductsAiSearchService
{
    public async Task<QueryResponse> ProductsChatAsync(string userQuery, CancellationToken cancellationToken)
    {
        return await productsRagService.RespondAsync(userQuery, cancellationToken);
    }
}