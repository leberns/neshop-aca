using Contracts.Products.Entities;
using Contracts.Review.Entities.Enums;

namespace Contracts.Review.Entities;

public record Review
{
    public int Id { get; init; }

    public int CustomerId { get; init; }
    public Customer.Entities.Customer Customer { get; init; } = null!;

    public int ProductId { get; init; }
    public Product Product { get; init; } = null!;

    public string? Text { get; init; }
    public RatingType Rating { get; init; }
    public ReviewStatusType Status { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}