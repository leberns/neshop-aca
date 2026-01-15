using Contracts.DataModels;
using Database.DataSeed;
using Microsoft.EntityFrameworkCore;

namespace Database;

public sealed class ShopDbContext(
    DbContextOptions<ShopDbContext> options,
    IEnumerable<ISeeder> seeders
) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<ProductCart> ProductCarts { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Category>().ToTable("Categories");

        modelBuilder.Entity<Review>().ToTable("Reviews");

        modelBuilder.Entity<Product>(builder =>
        {
            builder.HasIndex(b => b.Name);
            builder.HasIndex(b => b.Price);
        });

        foreach (var seeder in seeders)
        {
            seeder.Seed(modelBuilder);
        }
    }
}