using Contracts.Carts.ApiModels;

namespace Contracts.Carts.Services;

public interface ICartReader
{
    Task<CartInfo> GetCustomerCart(CancellationToken cancellationToken);
}