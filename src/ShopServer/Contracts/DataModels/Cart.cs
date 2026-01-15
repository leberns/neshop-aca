using Contracts.DataModels.Enums;

namespace Contracts.DataModels;

public record Cart
{
    public int Id { get; init; }

    public CartStatusType StatusType { get; init; } = CartStatusType.Open;

    public int CustomerId { get; init; }

    public Customer Customer { get; init; } = null!;

    public ICollection<ProductCart> ProductCarts { get; set; } = [];
}