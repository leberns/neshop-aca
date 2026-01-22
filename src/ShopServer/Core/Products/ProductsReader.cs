using Contracts.Products.ApiModels;
using Contracts.Products.Filters;
using Contracts.Products.Services;
using Database;

namespace Core.Products;

public class ProductsReader(
    ProductsRepository repository
    ) : IProductsReader
{
    public async Task<List<ProductInfo>> GetProducts(
        ProductsFilter filter,
        CancellationToken cancellationToken)
    {
        var products = await repository.GetProductsByFilter(filter, cancellationToken);

        return products.Select(p => p.ToProductInfo()).ToList();
    }

    public async Task<ProductInfo> GetProductById(int productId, CancellationToken cancellationToken)
    {
        return (await repository.GetProductById(productId, cancellationToken)).ToProductInfo();
    }
}
