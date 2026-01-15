using ShopWeb.ShopClient;

namespace WebContracts;

public interface IProductsService
{
    Task<List<ProductInfo>> ListProducts(CancellationToken cancellationToken);

    Task<ProductInfo> GetProductById(int productId, CancellationToken cancellationToken);
}