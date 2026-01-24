using Contracts.Carts.Entities;
using Contracts.Carts.Entities.Enums;
using Microsoft.EntityFrameworkCore;

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