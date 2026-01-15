using Contracts.ApiModels;

namespace Contracts.Services;

public interface ICartReader
{
    Task<CartInfo> GetCustomerCart(CancellationToken cancellationToken);
}