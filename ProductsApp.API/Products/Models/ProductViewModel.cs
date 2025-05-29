namespace ProductsApp.API.Products.Models;

public record ProductViewModel
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required string Color { get; init; }

    public required string CreatedBy { get; init; }
}