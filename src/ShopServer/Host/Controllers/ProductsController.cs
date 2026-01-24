using Microsoft.AspNetCore.Mvc;
using Contracts.ApiModels;
using Contracts.ApiModels.Filters;
using Contracts.Services;

namespace Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet(Name = "GetProducts")]
    public async Task<IEnumerable<ProductInfo>> Get(
        decimal? fromPrice,
        decimal? toPrice,
        int? brand,
        IProductsReader productsReader,
        CancellationToken cancellationToken
        )
    {
        var filter = new ProductsFilter
        {
            FromPrice = fromPrice,
            ToPrice = toPrice,
            BrandId = brand,
        };

        return await productsReader.GetProducts(filter, cancellationToken);
    }

    [HttpGet("{productId}", Name = "GetProductById")]
    public async Task<ProductInfo> GetProductById(
        int productId,
        IProductsReader productsReader,
        CancellationToken cancellationToken
    )
    {
        return await productsReader.GetProductById(productId, cancellationToken);
    }
}
