namespace Contracts.Products.ApiModels;

public record ProductInfo
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Price { get; set; } = string.Empty; // if Price is decimal NSwag generates ShopClient with double for it!

    public string CategoryName { get; set; } = string.Empty;

    public int BrandId { get; set; }

    public ICollection<string> ProductImages { get; set; } = [];
}