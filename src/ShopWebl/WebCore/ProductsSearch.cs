using ShopWeb.ShopClient;
using WebContracts;

namespace WebCore;

public class ProductsSearch(
    IShopClient shopClient
    ) : IProductsSearch
{
    public async Task<UserSearchResponse> Search(string userQuery, CancellationToken cancellationToken)
    {
        var request = new UserSearchRequest { UserQuery = userQuery };

        return await shopClient.SearchProductsAsync(request, cancellationToken);
    }
}