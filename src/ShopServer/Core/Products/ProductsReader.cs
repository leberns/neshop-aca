using Microsoft.Extensions.Logging;
using Contracts.Products.ApiModels;
using Contracts.Products.Filters;
using Contracts.Products.Repositories;
using Contracts.Products.Services;

namespace Core.Products;

public partial class ProductsReader(
    ILogger<ProductsReader> logger,
    IProductRepository repository
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

        var product = await repository.GetProductById(productId, cancellationToken);

        return product.ToProductInfo();
    }
}