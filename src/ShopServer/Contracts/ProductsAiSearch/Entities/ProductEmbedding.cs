using Contracts.Products.Entities;
using Pgvector;

namespace Contracts.ProductsAiSearch.Entities;

public record ProductEmbedding
{
    public required int Id { get; init; }

    public required int ProductId { get; init; }

    public Product Product { get; init; } = null!;

    public required string Content { get; init; }

    public required decimal Price { get; init; }

    public required string Category { get; init; }

    public required string Brand { get; init; }

    public required DateTime GeneratedOn { get; init; }

    public required string Deployment { get; init; }

    public required Vector Embedding { get; init; }
}