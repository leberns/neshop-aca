using Contracts.Products.ApiModels;
using Contracts.Products.Filters;
using Contracts.Products.Services;

namespace Host.Api;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Products");

        group.MapGet("/", async (
                decimal? fromPrice,
                decimal? toPrice,
                int? brand,
                IProductsReader productsReader,
                CancellationToken cancellationToken) =>
            {
                var filter = new ProductsFilter
                {
                    FromPrice = fromPrice,
                    ToPrice = toPrice,
                    BrandId = brand,
                };

                return await productsReader.GetProducts(filter, cancellationToken);
            })
            .WithName("GetProducts")
            .Produces<IEnumerable<ProductInfo>>(StatusCodes.Status200OK);

        group.MapGet("/{productId}", async (
                int productId,
                IProductsReader productsReader,
                CancellationToken cancellationToken)
                => await productsReader.GetProductById(productId, cancellationToken))
            .WithName("GetProductById")
            .Produces<ProductInfo>(StatusCodes.Status200OK);
    }
}