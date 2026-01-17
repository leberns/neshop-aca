using Contracts.ApiModels;
using Contracts.ApiModels.Filters;
using Contracts.Services;
using Database;

namespace Core.ReadProduct;

public class ProductsReader(
    ShopRepository repository
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
