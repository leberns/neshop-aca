using Contracts.ApiModels;
using Contracts.ApiModels.Filters;

namespace Contracts.Services;

public interface IProductsReader
{
    Task<List<ProductInfo>> GetProducts(
        ProductsFilter filter,
        CancellationToken cancellationToken);

    Task<ProductInfo> GetProductById(
        int productId,
        CancellationToken cancellationToken);
}