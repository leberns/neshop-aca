using Contracts.Products.Entities;
using Contracts.ProductsAiSearch.Entity;
using Contracts.ProductsAiSearch.Repositories;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace Database;

public class ProductRepositoryAiSearch(
    AppDbContext dbContext
    ) : IProductRepositoryAiSearch
{
    public async Task<List<Product>> GetSearchableProducts(CancellationToken cancellationToken)
    {
        return await dbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyProductEmbeddings(CancellationToken cancellationToken)
    {
        return await dbContext.ProductEmbeddings.AnyAsync(cancellationToken);
    }

    public async Task<List<Product>> SearchSimilarProducts(
        float[] vector,
        int limit,
        CancellationToken cancellationToken)
    {
        var pgVector = new Vector(vector);

        return await dbContext.ProductEmbeddings
            .OrderBy(e => e.Vector.L2Distance(pgVector))
            .Take(limit)
            .Select(e => e.Product)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
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