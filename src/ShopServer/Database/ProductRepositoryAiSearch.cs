using Contracts.Products.Entities;
using Contracts.ProductsAiSearch.Entities;
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
            .Include(p => p.Brand)
            .Include(p => p.Category)
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
            .Select(e => new
            {
                ProductEmbedding = e,
                Similarity = 1 - e.Embedding.CosineDistance(pgVector),
                Product = e.Product
            })
            .OrderBy(e => e.Similarity)
            .Take(limit)
            .Include(e => e.Product).ThenInclude(p => p.Brand)
            .Include(e => e.Product).ThenInclude(p => p.Category)
            .Include(e => e.Product).ThenInclude(p => p.Images)
            .Select(e => e.Product)
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