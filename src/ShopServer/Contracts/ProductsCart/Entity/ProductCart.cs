using Contracts.Products.Entities;

namespace Contracts.ProductsCart.Entity;

public record ProductCart
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal BundlePrice { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public int CartId { get; set; }

    public Carts.Entities.Cart Cart { get; set; } = null!;
}
