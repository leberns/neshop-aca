namespace Contracts.ApiModels.Filters;

public record ProductsFilter
{
    public decimal? FromPrice { get; init; }
    public decimal? ToPrice { get; init; }
    public int? BrandId { get; init; }
}