using ProductsApp.API.Products.Models;

namespace ProductsApp.API.Products.Managers;

public interface IProductsManager
{
    Task<IReadOnlyCollection<ProductViewModel>> GetAll(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<ProductViewModel>> QueryProducts(
        string? name,
        string? color,
        string? createdBy,
        CancellationToken cancellationToken);

    Task<ProductViewModel> CreateProduct(
        string name,
        string color,
        CancellationToken cancellationToken);

    Task<ProductViewModel?> UpdateProduct(
        int id,
        string name,
        string color,
        CancellationToken cancellationToken);
    
    Task<bool> DeleteProduct(
        int id,
        CancellationToken cancellationToken);

    Task<ProductViewModel?> GetById(
        int id,
        CancellationToken cancellationToken);
}