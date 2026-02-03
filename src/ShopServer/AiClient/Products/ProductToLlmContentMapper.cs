using Contracts.Products.Entities;

namespace AiClient.Products;

public static class ProductToLlmContentMapper
{
    /// <summary>
    /// Build the content to be embedded based on the relevant product fields.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public static string ToEmbeddingContent(this Product product)
        => $"""
            - Name: {product.Name}
              Description: {product.Description}
              Category: {product.Category.Name}
              Brand: {product.Brand.Name}
            """;

    /// <summary>
    /// Build the content to be used in the chat about the relevant products.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public static string ToAssistantContent(this Product product)
        => $"""
            - Name: {product.Name}
              Description: {product.Description}
              Category: {product.Category.Name}
              Brand: {product.Brand.Name}
              Price: ${product.Price:F2}
            """;
}