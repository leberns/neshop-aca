using ShopWeb.ShopClient;

namespace WebContracts;

public interface IAppStateService
{
    List<ProductInfo> Products { get; set; }

    public ProductInfo? SelectedProduct { get; set; }
}