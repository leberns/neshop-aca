namespace Contracts.ProductsAiSearch.ApiModels;

public record QueryResponse
{
    public required string Text { get; init; }
}