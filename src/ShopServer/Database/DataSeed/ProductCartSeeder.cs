using Microsoft.EntityFrameworkCore;
using Contracts.DataModels;

namespace Database.DataSeed;

public class ProductCartSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCart>().HasData(CreateData());
    }

    private static List<ProductCart> CreateData()
    {
        return
        [
            new ProductCart
            {
                Id = 1,
                Quantity = 1,
                UnitPrice = 315.00m,
                BundlePrice = 315.00m,
                ProductId = 1,
                CartId = 1,
            },
            new ProductCart
            {
                Id = 2,
                Quantity = 3,
                UnitPrice = 75.00m,
                BundlePrice = 150.00m,
                ProductId = 7,
                CartId = 1,
            },
        ];
    }
}