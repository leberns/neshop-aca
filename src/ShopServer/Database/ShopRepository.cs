using Microsoft.EntityFrameworkCore;
using Contracts.ApiModels.Filters;
using Contracts.DataModels;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace Database;

public class ShopRepository(ShopDbContext dbContext)
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

    public async Task<List<Product>> SearchSimilarProducts(
        Vector vector,
        int limit,
        CancellationToken cancellationToken)
    {
        return await dbContext.ProductEmbeddings
            .OrderBy(e => e.Vector.L2Distance(vector))
            .Take(limit)
            .Select(e => e.Product)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
    {
        return await dbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductEmbedding?> FindProductEmbeddingById(int productId, CancellationToken cancellationToken)
    {
        return await dbContext.ProductEmbeddings
            .FirstOrDefaultAsync(e => e.ProductId == productId, cancellationToken);
    }

    public async Task AddProductEmbedding(ProductEmbedding productEmbedding, CancellationToken cancellationToken)
    {
        await dbContext.ProductEmbeddings.AddAsync(productEmbedding, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateProductEmbedding(ProductEmbedding productEmbedding, CancellationToken cancellationToken)
    {
        dbContext.ProductEmbeddings.Update(productEmbedding);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}