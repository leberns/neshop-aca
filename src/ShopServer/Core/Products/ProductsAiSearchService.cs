using AiClient.Interfaces;
using Contracts.Products.Services;
using Contracts.ProductsAiSearch.ApiModels;

namespace Core.Products;

public class ProductsAiSearchService(
    IProductRagService productsRagService
    ) : IProductsAiSearchService
{
    public async Task<UserSearchResponse> ProductsSearchAsync(string userQuery, CancellationToken cancellationToken)
    {
        var ragResponse = await productsRagService.RespondAsync(userQuery, cancellationToken);

        return new UserSearchResponse
        {
            Text = ragResponse.Text,
            Products = ragResponse.Products.Select(p => p.ToProductInfo()).ToList(),
        };
    }
}