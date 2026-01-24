using Contracts.Categories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DataSeed;

public class CategorySeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(CreateData());
    }

    private static List<Category> CreateData()
    {
        return [
            new Category { Id = 1, Name = "Tent" },
            new Category { Id = 2, Name = "Hiking Shoes" },
            new Category { Id = 3, Name = "Backpacks" }
        ];
    }
}