using Contracts.Products.ApiModels;
using Contracts.Products.Services;
using Contracts.ProductsAiSearch.ApiModels;
using Contracts.ProductsAiSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.Api;

public static class ProductSearchEndpoints
{
    public static void MapProductSearchEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Products/Search");

        group.MapPost("/", async (
                [FromBody] UserQueryRequest userQueryRequest,
                IProductsSearchService productsSearchService,
                CancellationToken cancellationToken) =>
            await productsSearchService.SearchProductsAsync(userQueryRequest.UserQuery, cancellationToken))
            .WithName("SearchProducts")
            .Produces<IEnumerable<ProductInfo>>();
    }
}