using Contracts.Products.Entities;

namespace Contracts.Images.Entities;

public record Image
{
    public int Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;
};