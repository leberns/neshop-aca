using Contracts.Products.Entities;

namespace Contracts.ProductsAiSearch.Models;

public record RagResponse
{
    public required string Text { get; init; }
    public required List<Product> Products { get; init; }
}