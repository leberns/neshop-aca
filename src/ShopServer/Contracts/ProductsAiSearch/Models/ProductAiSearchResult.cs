using Contracts.Products.Entities;

namespace Contracts.ProductsAiSearch.Models;

public record ProductAiSearchResult(Product Product, double Similarity);