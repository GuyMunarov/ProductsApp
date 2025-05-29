using ProductsApp.API.Products.Models;
using ProductsApp.Domain;

namespace ProductsApp.API.Products.Mappers;

public static class ProductMapper
{
    public static ProductViewModel Map(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Color = product.Color,
        CreatedBy = product.CreatedBy.Username
    };
}