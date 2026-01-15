using Microsoft.EntityFrameworkCore;
using Contracts.DataModels;
using Contracts.DataModels.Enums;

namespace Database.DataSeed;

public class ReviewSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>().HasData(CreateData());
    }

    private static List<Review> CreateData()
    {
        return [
            new Review
            {
                Id = 1,
                CustomerId = 1,
                ProductId = 1,
                Text = "Great product!",
                Rating = RatingType.Excellent,
                Status = ReviewStatusType.Active,
                CreatedAt = new DateTimeOffset( new DateOnly(2025, 02, 14), new TimeOnly(20, 35), TimeSpan.FromHours(1)),
                UpdatedAt = new DateTimeOffset( new DateOnly(2025, 02, 14), new TimeOnly(20, 35), TimeSpan.FromHours(1)),
            },
            new Review
            {
                Id = 2,
                CustomerId = 2,
                ProductId = 1,
                Text = null,
                Rating = RatingType.Fair,
                Status = ReviewStatusType.Archived,
                CreatedAt = new DateTimeOffset( new DateOnly(2025, 02, 15), new TimeOnly(13, 30), TimeSpan.FromHours(1)),
                UpdatedAt = new DateTimeOffset( new DateOnly(2025, 02, 16), new TimeOnly(17, 34), TimeSpan.FromHours(1)),
            },
            new Review
            {
                Id = 3,
                CustomerId = 2,
                ProductId = 1,
                Text = "Too small for big people!",
                Rating = RatingType.Poor,
                Status = ReviewStatusType.Active,
                CreatedAt = new DateTimeOffset( new DateOnly(2025, 02, 16), new TimeOnly(17, 35), TimeSpan.FromHours(1)),
                UpdatedAt = new DateTimeOffset( new DateOnly(2025, 02, 16), new TimeOnly(17, 35), TimeSpan.FromHours(1)),
            },
        ];
    }
}