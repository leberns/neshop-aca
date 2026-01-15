using Microsoft.EntityFrameworkCore;
using Contracts.DataModels;

namespace Database.DataSeed;

public class CustomerSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(CreateData());
    }

    private static List<Customer> CreateData()
    {
        return [
            new Customer { Id = 1, UserName = "John Blue", Email = "john.blue@example.ch"},
            new Customer { Id = 2, UserName = "Marry Green", Email = "marry.green@example.ch"},
        ];
    }
}