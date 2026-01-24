using Contracts.Products.ApiModels;
using Contracts.Products.Filters;

namespace Contracts.Products.Services;

public interface IProductsReader
{
    Task<List<ProductInfo>> GetProducts(
        ProductsFilter filter,
        CancellationToken cancellationToken);

    Task<ProductInfo> GetProductById(
        int productId,
        CancellationToken cancellationToken);
}