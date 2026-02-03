using Microsoft.EntityFrameworkCore;
using Contracts.Products.Entities;
using Contracts.Products.Filters;
using Contracts.Products.Repositories;

namespace Database;

public class ProductRepository(
    AppDbContext dbContext
    ) : IProductRepository
{
    public async Task<List<Product>> GetProductsByFilter(
        ProductsFilter filter,
        CancellationToken cancellationToken
        )
    {
        IQueryable<Product> query = dbContext.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images);

        if (filter.FromPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.FromPrice);
        }

        if (filter.ToPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.ToPrice);
        }

        if (filter.BrandId.HasValue)
        {
            query = query.Where(p => p.BrandId == filter.BrandId);
        }

        var products = await query.ToListAsync(cancellationToken);

        return products;
    }

    public async Task<Product> GetProductById(
        int productId,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Product> query = dbContext.Products
            .Where(p => p.Id == productId)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images);

        return await query.FirstOrDefaultAsync(cancellationToken)
               ?? throw new InvalidOperationException($"Product with ID {productId} not found");
    }
}