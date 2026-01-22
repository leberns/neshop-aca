using System.Globalization;
using Contracts.Products.ApiModels;
using Contracts.Products.Entities;

namespace Core.Products;

public static class ProductToProductInfo
{
    extension(Product product)
    {
        public ProductInfo ToProductInfo()
        => new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.ToString(CultureInfo.InvariantCulture),
            CategoryName = product.Category.Name,
            BrandId = product.BrandId,
            ProductImages = product.Images.Select(pi => pi.Name).ToList()
        };
    }
}