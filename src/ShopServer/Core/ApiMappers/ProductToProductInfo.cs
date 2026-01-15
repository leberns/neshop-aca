using System.Globalization;
using Contracts.ApiModels;
using Contracts.DataModels;

namespace Core.ApiMappers;

public static class ProductToProductInfo
{
    public static ProductInfo ToProductInfo(this Product product)
    {
        return new ProductInfo
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