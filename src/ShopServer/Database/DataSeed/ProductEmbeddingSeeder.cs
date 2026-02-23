using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Contracts.ProductsAiSearch.Entities;

namespace Database.DataSeed;

public class ProductEmbeddingSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEmbedding>().HasData(CreateData());
    }

    private static List<ProductEmbedding> CreateData()
    {
        var basePath = Directory.GetCurrentDirectory();
        var contentPath = Path.Combine(basePath, "Migrations", "ProductEmbeddings.json");
        var contentJson = File.ReadAllText(contentPath);
        var contentItems = JsonSerializer.Deserialize<ProductEmbedding[]>(contentJson)!;

        return contentItems.ToList();
    }
}