using Contracts;
using Pgvector;
using Contracts.Brands.Entities;
using Contracts.Carts.Entities;
using Contracts.Categories.Entities;
using Contracts.Customers.Entities;
using Contracts.Images.Entities;
using Contracts.Products.Entities;
using Contracts.ProductsAiSearch.Entity;
using Contracts.ProductsCart.Entity;
using Contracts.Reviews.Entities;
using Database.DataSeed;
using Microsoft.EntityFrameworkCore;

namespace Database;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IEnumerable<ISeeder> seeders
) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductEmbedding> ProductEmbeddings { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<ProductCart> ProductCarts { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(o => o.UseVector());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

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

        modelBuilder.Entity<ProductEmbedding>(builder =>
        {
            builder.Property(e => e.Embedding)
                .HasColumnType(Constants.AiAzureEmbedding.DbType);
        });

        foreach (var seeder in seeders)
        {
            seeder.Seed(modelBuilder);
        }
    }
}