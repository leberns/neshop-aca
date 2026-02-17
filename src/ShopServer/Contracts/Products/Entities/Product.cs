using Contracts.Categories.Entities;
using Contracts.Brands.Entities;
using Contracts.Images.Entities;
using Contracts.ProductsCart.Entities;

namespace Contracts.Products.Entities;

public record Product
{
    public required int Id { get; set; }

    public required string Name { get; set; } = string.Empty;

    public required string Description { get; set; } = string.Empty;

    public required decimal Price { get; set; }

    public DateTimeOffset? CreatedOn { get; init; }

    public DateTimeOffset? UpdatedOn { get; init; }

    public required int BrandId { get; set; }

    public Brand Brand { get; set; } = null!;

    public required int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<Image> Images { get; } = [];

    public ICollection<ProductCart> ProductCarts { get; set; } = [];
}