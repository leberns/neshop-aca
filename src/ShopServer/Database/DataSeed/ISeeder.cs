using Microsoft.EntityFrameworkCore;

namespace Database.DataSeed;

public interface ISeeder
{
    /// <summary>
    /// Add default and sample data to the database
    /// </summary>
    void Seed(ModelBuilder modelBuilder);
}