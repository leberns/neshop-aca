using System.Text.Json;
using System.Text.Json.Serialization;
using Contracts.ProductsAiSearch.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pgvector;

namespace Database.DataSeed;

/// <summary>
/// As HasData do not support Vector for seeding, use the extension method called at runtime to populate the database.
/// </summary>
public static class ProductEmbeddingRuntimeSeeder
{
    public static async Task SeedProductEmbeddingsAsync(
        this AppDbContext context,
        ILogger logger)
    {
        logger.LogInformation("Checking for product embeddings seeding");

        if (await context.ProductEmbeddings.AnyAsync())
        {
            logger.LogInformation("Product embeddings already seeded");
            return;
        }

        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var path = Path.Combine(basePath, "DataSeed", "ProductEmbeddings.json");

        if (!File.Exists(path))
        {
            logger.LogError("Product embedding seed file not found: {Path}", path);
            return;
        }

        var contentJson = await File.ReadAllTextAsync(path);

        logger.LogInformation("Product embeddings size: {ContentJsonLength}", contentJson.Length);

        var options = new JsonSerializerOptions();
        options.Converters.Add(new VectorJsonConverter());
        var embeddings = JsonSerializer.Deserialize<ProductEmbedding[]>(contentJson, options);

        if (embeddings is not null && embeddings.Length > 0)
        {
            logger.LogInformation("Found {EmbeddingsLength} product embeddings", embeddings.Length);

            await context.ProductEmbeddings.AddRangeAsync(embeddings);
            await context.SaveChangesAsync();
        }

        logger.LogInformation("Finished seeding product embeddings");
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
