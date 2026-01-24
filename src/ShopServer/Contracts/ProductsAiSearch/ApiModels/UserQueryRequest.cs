namespace Contracts.ProductsAiSearch.ApiModels;

public record UserQueryRequest()
{
    public string UserQuery { get; set; } = string.Empty;
}