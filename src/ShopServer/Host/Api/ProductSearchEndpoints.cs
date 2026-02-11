using Contracts.Products.Services;
using Contracts.ProductsAiSearch.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace Host.Api;

public static class ProductSearchEndpoints
{
    public static void MapProductSearchEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Products/Search");

        group.MapPost("/", async (
            [FromBody] UserSearchRequest userSearchRequest,
            IProductsAiSearchService productsAiSearchService,
            CancellationToken cancellationToken)
                => await productsAiSearchService.ProductsSearchAsync(userSearchRequest.UserQuery, cancellationToken))
            .WithName("SearchProducts")
            .Produces<UserSearchResponse>();
    }
}