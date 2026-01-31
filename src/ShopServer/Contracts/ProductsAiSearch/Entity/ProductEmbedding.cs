using Pgvector;
using Contracts.Products.Entities;

namespace Contracts.ProductsAiSearch.Entity;

public record ProductEmbedding
{
    public required int Id { get; init; }

    public required int ProductId { get; init; }

    public required Product Product { get; init; }

    public required string Content { get; init; }

    public required decimal Price { get; init; }

    public required string Category { get; init; }

    public required string Brand { get; init; }

    public required DateTime GeneratedAtUtc { get; init; }

    public required string Model { get; init; }

    public required Vector Embedding { get; init; }
}