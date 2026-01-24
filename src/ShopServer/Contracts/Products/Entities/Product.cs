using Contracts.Categories.Entities;
using Contracts.Brands.Entities;
using Contracts.Images.Entities;
using Contracts.ProductsCart.Entity;

namespace Contracts.Products.Entities;

public record Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; } = 0;

    public DateTimeOffset CreatedOn { get; set; }

    public int BrandId { get; set; }

    public Brand Brand { get; set; } = null!;

    public int CategoryId { get; set; } = 0;

    public Category Category { get; set; } = null!;

    public ICollection<Image> Images { get; } = [];

    public ICollection<ProductCart> ProductCarts { get; set; } = [];
}