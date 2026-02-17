using Contracts.Brands.Entities;
using Contracts.Categories.Entities;
using Contracts.Products.Entities;

namespace Core.Tests.TestData;

public static class Products
{
    public static Product MakeProduct(int id, string name, decimal price)
    {
        return new Product
        {
            Id = id,
            Name = name,
            Description = "Test Description",
            Price = price,
            CreatedOn = new DateTime(2026, 1, 12),
            UpdatedOn = new DateTime(2026, 1, 12),
            BrandId = 1,
            CategoryId = 1,
            Brand = new Brand { Id = 1, Name = "Test Brand" },
            Category = new Category { Id = 1, Name = "Test Category" }
        };
    }

}