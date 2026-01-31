using Contracts.Products.Entities;

namespace AiClient.Products;

public static class ProductEmbeddingContentMapper
{
    public static string ToEmbeddingContent(this Product product)
    => $"""
        Product name: {product.Name}
        Category: {product.Category.Name}
        Brand: {product.Brand.Name}
        Description: {product.Description}
        """;
}