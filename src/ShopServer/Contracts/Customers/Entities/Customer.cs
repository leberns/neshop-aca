namespace Contracts.Customers.Entities;

public record Customer
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ICollection<Carts.Entities.Cart> Carts { get; set; } = [];
}