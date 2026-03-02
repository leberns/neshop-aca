using Contracts.Products.Entities;
using Contracts.Products.Filters;
using Contracts.Products.Repositories;
using Core.Products;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests;

public class ProductsReaderTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly ProductsReader _reader;

    public ProductsReaderTests()
    {
        var loggerMock = new Mock<ILogger<ProductsReader>>();
        _repositoryMock = new Mock<IProductRepository>();
        _reader = new ProductsReader(loggerMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsProductInfoList()
    {
        var filter = new ProductsFilter { FromPrice = 100 };
        var products = new List<Product>
        {
            TestData.Products.MakeProduct(1, "Product 1", 100),
            TestData.Products.MakeProduct(2, "Product 2", 200)
        };

        _repositoryMock
            .Setup(r => r.GetProductsByFilter(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var result = await _reader.GetProducts(filter, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Product 1", result[0].Name);
        Assert.Equal("Product 2", result[1].Name);
        _repositoryMock.Verify(r => r.GetProductsByFilter(filter, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProducts_EmptyResult_ReturnsEmptyList()
    {
        var filter = new ProductsFilter();
        _repositoryMock
            .Setup(r => r.GetProductsByFilter(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        var result = await _reader.GetProducts(filter, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProductById_ExistingProduct_ReturnsProductInfo()
    {
        var product = TestData.Products.MakeProduct(1, "Test Product", 150);

        _repositoryMock
            .Setup(r => r.GetProductById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var result = await _reader.GetProductById(1, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Product", result.Name);
        Assert.Equal("150", result.Price);
        _repositoryMock.Verify(r => r.GetProductById(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProductById_NonExistingProduct_ThrowsException()
    {
        _repositoryMock
            .Setup(r => r.GetProductById(999, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Product with ID 999 not found"));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _reader.GetProductById(999, CancellationToken.None)
        );
    }
}