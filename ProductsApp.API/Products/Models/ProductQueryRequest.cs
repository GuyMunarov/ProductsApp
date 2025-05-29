namespace ProductsApp.API.Products.Models;

public record ProductQueryRequest(string? Name, string? Color, string? CreatedBy);