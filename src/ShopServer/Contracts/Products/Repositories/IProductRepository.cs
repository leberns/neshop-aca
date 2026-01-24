using Contracts.Products.Entities;
using Contracts.Products.Filters;

namespace Contracts.Products.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetProductsByFilter(ProductsFilter filter, CancellationToken cancellationToken);

    Task<Product> GetProductById(int productId, CancellationToken cancellationToken);
}