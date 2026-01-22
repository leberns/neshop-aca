using Contracts.Images.Entities;

namespace Contracts.Products.Entities;

public record Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; } = 0;

    public DateTimeOffset CreatedOn { get; set; }

    public int BrandId { get; set; }

    public Brand.Entities.Brand Brand { get; set; } = null!;

    public int CategoryId { get; set; } = 0;

    public Category.Entities.Category Category { get; set; } = null!;

    public ICollection<Image> Images { get; } = [];

    public ICollection<ProductCart.Entity.ProductCart> ProductCarts { get; set; } = [];
}