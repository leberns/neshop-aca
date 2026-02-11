using Contracts.Products.ApiModels;

namespace Contracts.ProductsAiSearch.ApiModels;

public record UserSearchResponse
{
    public required string Text { get; init; }
    public required List<ProductInfo> Products { get; init; }
}