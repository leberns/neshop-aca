namespace Contracts.ProductsAiSearch.ApiModels;

public record UserQueryRequest
{
    public required string UserQuery { get; init; }
}