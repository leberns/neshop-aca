using Contracts.Products.Entities;

namespace Contracts.Category.Entities;

public record Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = [];
};