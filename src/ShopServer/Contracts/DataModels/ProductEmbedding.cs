using Pgvector;

namespace Contracts.DataModels;

public class ProductEmbedding
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Vector Vector { get; set; } = null!;

    public string SourceText { get; set; } = "";

    public DateTime GeneratedAtUtc { get; set; }
    public string Model { get; set; } = "";
}