using Contracts.Cart.ApiModels;

namespace Contracts.Cart.Services;

public interface ICartReader
{
    Task<CartInfo> GetCustomerCart(CancellationToken cancellationToken);
}