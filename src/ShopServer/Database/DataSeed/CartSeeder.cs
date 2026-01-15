using Microsoft.EntityFrameworkCore;
using Contracts.DataModels;
using Contracts.DataModels.Enums;

namespace Database.DataSeed;

public class CartSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>().HasData(CreateData());
    }

    private static List<Cart> CreateData()
    {
        return
        [
            new Cart
            {
                Id = 1,
                StatusType = CartStatusType.Open,
                CustomerId = 1,
            }
        ];
    }
}