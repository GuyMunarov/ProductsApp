using Microsoft.EntityFrameworkCore;
using ProductsApp.API.Products.Mappers;
using ProductsApp.API.Products.Models;
using ProductsApp.Persistance;

namespace ProductsApp.API.Products.Managers;

internal class ProductsManager(AppDbContext dbContext) : IProductsManager
{
    public async Task<IReadOnlyCollection<ProductViewModel>> GetAll(CancellationToken cancellationToken)
    {
        var products = await QueryProducts(null, null, null, cancellationToken);
        return products;
    }
    
    public async Task<IReadOnlyCollection<ProductViewModel>> QueryProducts(
        string? name,
        string? color,
        string? createdBy,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .Where(p => p.Name == name || string.IsNullOrEmpty(name))
            .Where(p => p.Color == color || string.IsNullOrEmpty(color))
            .Where(p => p.CreatedBy.Username == createdBy || string.IsNullOrEmpty(createdBy))
            .Select(p => ProductMapper.Map(p))
            .ToArrayAsync(cancellationToken);

        return products;
    }

    public async Task<ProductViewModel?> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

        return product is not null 
            ? ProductMapper.Map(product) 
            : null;
    }
}