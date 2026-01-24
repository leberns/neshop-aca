namespace Contracts.DataModels;

public record ProductEmbedding
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public float[] Vector { get; set; } = [];

    public string SourceText { get; set; } = "";

    public DateTime GeneratedAtUtc { get; set; }
    public string Model { get; set; } = "";
}