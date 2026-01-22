using Contracts.Cart.Entities.Enums;

namespace Contracts.Cart.Entities;

public record Cart
{
    public int Id { get; init; }

    public CartStatusType StatusType { get; init; } = CartStatusType.Open;

    public int CustomerId { get; init; }

    public Customer.Entities.Customer Customer { get; init; } = null!;

    public ICollection<ProductCart.Entity.ProductCart> ProductCarts { get; set; } = [];
}