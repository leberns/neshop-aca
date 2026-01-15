using ShopWeb.ShopClient;
using WebContracts;

namespace WebCore;

public class ProductsService(
    IShopClient shopClient
    ) : IProductsService
{
    public async Task<List<ProductInfo>> ListProducts(CancellationToken cancellationToken)
    {
        var products = await shopClient.GetProductsAsync(null, null, null, cancellationToken);

        return products.ToList();
    }

    public async Task<ProductInfo> GetProductById(int productId, CancellationToken cancellationToken)
        => await shopClient.GetProductByIdAsync(productId, cancellationToken);
}