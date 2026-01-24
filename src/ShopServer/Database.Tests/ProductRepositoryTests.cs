using Microsoft.EntityFrameworkCore;
using Contracts.Products.Filters;
using Database.DataSeed;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Tests;

public class ProductsRepositoryTests : IDisposable
{
    private readonly AppDbContext _dbContext;
    private readonly ProductRepository _repository;

    public ProductsRepositoryTests()
    {
        var services = new ServiceCollection();

        services.AddSeeders();

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: "test_neshopdb")
        );

        var serviceProvider = services.BuildServiceProvider();

        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        _repository = new ProductRepository(_dbContext);

        _dbContext.Database.EnsureCreated();
    }

    [Theory]
    [InlineData(150, null, null, 5)]
    [InlineData(null, 200, null, 4)]
    [InlineData(null, null, 1, 2)]
    [InlineData(100, 200, 1, 1)]
    public async Task GetProductsByFilter_ReturnsProducts(
        int? fromPrice,
        int? toPrice,
        int? brandId,
        int expectedCount)
    {
        var filter = new ProductsFilter
        {
            FromPrice = fromPrice,
            ToPrice = toPrice,
            BrandId = brandId
        };

        var result = await _repository.GetProductsByFilter(filter, CancellationToken.None);

        Assert.Equal(expectedCount, result.Count);

        if (fromPrice.HasValue)
        {
            Assert.All(result, p => Assert.True(p.Price >= fromPrice));
        }

        if (toPrice.HasValue)
        {
            Assert.All(result, p => Assert.True(p.Price <= toPrice));
        }

        if (brandId.HasValue)
        {
            Assert.All(result, p => Assert.True(p.BrandId == brandId));
        }
    }

    [Fact]
    public async Task GetProductById_ExistingProduct_ReturnsProduct()
    {
        var result = await _repository.GetProductById(1, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.NotNull(result.Brand);
        Assert.NotNull(result.Category);
    }

    [Fact]
    public async Task GetProductById_NonExistingProduct_ThrowsException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _repository.GetProductById(999, CancellationToken.None)
        );
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}