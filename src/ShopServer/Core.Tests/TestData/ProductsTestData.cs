using Contracts.Brands.Entities;
using Contracts.Categories.Entities;
using Contracts.Products.Entities;

namespace Core.Tests.TestData;

public static class ProductsTestData
{
    public static Product CreateTestProduct(int id, string name, decimal price)
    {
        return new Product
        {
            Id = id,
            Name = name,
            Description = "Test Description",
            Price = price,
            BrandId = 1,
            CategoryId = 1,
            Brand = new Brand { Id = 1, Name = "Test Brand" },
            Category = new Category { Id = 1, Name = "Test Category" }
        };
    }

}