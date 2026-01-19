using Contracts.ApiModels;
using Contracts.ApiModels.Filters;
using Contracts.Services;
using Core.ApiMappers;
using Database;
using Microsoft.Extensions.Logging;

namespace Core;

public class ProductsReader(
    ILogger<ProductsReader> logger,
    ShopRepository repository
    ) : IProductsReader
{
    public async Task<List<ProductInfo>> GetProducts(
        ProductsFilter filter,
        CancellationToken cancellationToken)
    {
        var products = await repository.GetProductsByFilter(filter, cancellationToken);

        logger.LogInformation("GetProducts: Retrieved {ProductsCount} products", products.Count);

        return products.Select(p => p.ToProductInfo()).ToList();
    }

    public async Task<ProductInfo> GetProductById(int productId, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductById {ProductId}", productId);

        return (await repository.GetProductById(productId, cancellationToken)).ToProductInfo();
    }
}
