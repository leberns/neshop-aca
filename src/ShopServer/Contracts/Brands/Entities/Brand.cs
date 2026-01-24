using Contracts.Products.Entities;

namespace Contracts.Brands.Entities;

public record Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = [];
};