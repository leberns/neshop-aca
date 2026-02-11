using ShopWeb.ShopClient;

namespace WebContracts;

public interface IProductsSearch
{
    Task<UserSearchResponse> Search(string userQuery, CancellationToken cancellationToken);
}