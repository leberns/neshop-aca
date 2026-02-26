using System.Text.Json;
using System.Text.Json.Serialization;
using Contracts.ProductsAiSearch.Entities;
using Microsoft.EntityFrameworkCore;
using Pgvector;

namespace Database.DataSeed;

/// <summary>
/// As HasData do not support Vector for seeding, use the extension method called at runtime to populate the database.
/// </summary>
public static class ProductEmbeddingRuntimeSeeder
{
    public static async Task SeedProductEmbeddingsAsync(this AppDbContext context)
    {
        if (await context.ProductEmbeddings.AnyAsync())
        {
            return;
        }

        var basePath = Directory.GetCurrentDirectory();
        var path = Path.Combine(basePath, "..", "Database", "Migrations", "ProductEmbeddings.json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Product embedding seed file not found: {path}");
        }

        var contentJson = await File.ReadAllTextAsync(path);

        var options = new JsonSerializerOptions();
        options.Converters.Add(new VectorJsonConverter());
        var embeddings = JsonSerializer.Deserialize<ProductEmbedding[]>(contentJson, options);

        if (embeddings is not null && embeddings.Length > 0)
        {
            await context.ProductEmbeddings.AddRangeAsync(embeddings);
            await context.SaveChangesAsync();
        }
    }

    private class VectorJsonConverter : JsonConverter<Vector>
    {
        public override Vector Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType != JsonTokenType.String
                ? throw new JsonException("Expected string array for Vector")
                : new Vector(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Vector value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
