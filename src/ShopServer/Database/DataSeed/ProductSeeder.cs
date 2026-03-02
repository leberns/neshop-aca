using Microsoft.EntityFrameworkCore;
using Contracts.Products.Entities;

namespace Database.DataSeed;

public class ProductSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(CreateData());
    }

    private static List<Product> CreateData()
    {
        return
        [
            new Product
            {
                Id = 1,
                Name = "Aura 2 Ultralight",
                BrandId = 1,
                CategoryId = 1,
                Price = 315.00m,
                CreatedOn = null,
                UpdatedOn = null,
                Description = "Fictional ultralight tent for high-altitude camping."
            },
            new Product
            {
                Id = 2,
                Name = "Citadel Fortress 2",
                BrandId = 2,
                CategoryId = 1,
                Price = 285.99m,
                CreatedOn = null,
                UpdatedOn = null,
                Description = "A 2-person heavy-duty basecamp tent built for extreme conditions."
            },
            new Product
            {
                Id = 3,
                Name = "Citadel Fortress 4",
                BrandId = 2,
                CategoryId = 1,
                Price = 485.99m,
                CreatedOn = null,
                UpdatedOn = null,
                Description = "A heavy-duty basecamp tent built for extreme conditions."
            },
            new Product
            {
                Id = 4,
                Name = "Vanguard Low-Pro",
                BrandId = 1,
                CategoryId = 2,
                Price = 145.00m,
                CreatedOn = null,
                UpdatedOn = null,
                Description = "Synthetic mesh trail runners for rapid elevation gain."
            },
            new Product
            {
                Id = 5,
                Name = "Titan-Claw Boots",
                BrandId = 2,
                CategoryId = 2,
                Price = 189.50m,
                CreatedOn = null,
                UpdatedOn = null,
                Description = "Reinforced leather boots for technical rocky scrambles."
            },
            new Product
            {
                Id = 6,
                Name = "Atlas Hauler 75L",
                BrandId = 2,
                CategoryId = 3,
                Price = 265.00m,
                CreatedOn = null,
                UpdatedOn = null,
                Description = "Expedition-grade pack with modular external storage."
            },
            new Product
            {
                Id = 7,
                Name = "Swift-Pulse 15",
                BrandId = 2,
                CategoryId = 3,
                Price = 75.00m,
                CreatedOn = null,
                UpdatedOn = null,
                Description = "Minimalist hydration pack for peak bagging."
            },
            new Product
            {
                Id = 8,
                Name = "Nomad Versa-Pack",
                BrandId = 3,
                CategoryId = 3,
                Price = 99.00m,
                CreatedOn = null,
                UpdatedOn = null,
                Description = "Roll-top waterproof backpack for the modern explorer."
            }
        ];
    }
}