using Contracts.Products.ApiModels;
using Contracts.Products.Filters;
using Contracts.Products.Services;
using Database;
using Microsoft.Extensions.Logging;

namespace Core.Products;

public partial class ProductsReader(
    ILogger<ProductsReader> logger,
    ProductsRepository repository
    ) : IProductsReader
{
    public async Task<List<ProductInfo>> GetProducts(
        ProductsFilter filter,
        CancellationToken cancellationToken)
    {
        var products = await repository.GetProductsByFilter(filter, cancellationToken);

        LogProductsRetrieved(logger, nameof(GetProducts), products.Count);

        return products.Select(p => p.ToProductInfo()).ToList();
    }

    public async Task<ProductInfo> GetProductById(int productId, CancellationToken cancellationToken)
    {
        LogGetProductById(logger, nameof(GetProductById), productId);

        return (await repository.GetProductById(productId, cancellationToken)).ToProductInfo();
    }
}
