using Contracts.Brands.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DataSeed;

public class BrandSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>().HasData(CreateData());
    }

    private static List<Brand> CreateData()
    {
        return [
            new Brand { Id = 1, Name = "ApexRidge" },
            new Brand { Id = 2, Name = "TerraFlow" },
            new Brand { Id = 3, Name = "ZenithPath" }
        ];
    }
}