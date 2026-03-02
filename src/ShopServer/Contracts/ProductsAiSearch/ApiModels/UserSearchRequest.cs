namespace Contracts.ProductsAiSearch.ApiModels;

public record UserSearchRequest
{
    public required string UserQuery { get; init; }
}