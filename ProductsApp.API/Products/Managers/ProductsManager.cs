using Microsoft.EntityFrameworkCore;
using ProductsApp.API.Products.Mappers;
using ProductsApp.API.Products.Models;
using ProductsApp.Domain;
using ProductsApp.Infrastructure.Services;
using ProductsApp.Persistance;

namespace ProductsApp.API.Products.Managers;

internal class ProductsManager(AppDbContext dbContext, IUserService userService) : IProductsManager
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
            .Where(p => EF.Functions.Collate(p.Name.Trim(), "NOCASE") == name || string.IsNullOrEmpty(name))
            .Where(p => EF.Functions.Collate(p.Color.Trim(), "NOCASE") == color || string.IsNullOrEmpty(color))
            .Where(p => EF.Functions.Collate(p.CreatedBy.Username.Trim(), "NOCASE") == createdBy
                        || string.IsNullOrEmpty(createdBy))
            .Select(p => ProductMapper.Map(p))
            .ToArrayAsync(cancellationToken);

        return products;
    }

    public async Task<ProductViewModel> CreateProduct(string name, string color, CancellationToken cancellationToken)
    {
        var product = new Product(name, color, userService.UserId!.Value);
        var addedProductEntity = await dbContext.Products.AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var addedProduct = await GetById(addedProductEntity.Entity.Id, cancellationToken);
        return addedProduct!;
    }

    public async Task<ProductViewModel?> UpdateProduct(
        int id,
        string name,
        string color,
        CancellationToken cancellationToken)
    {
        var product = await FindUserProductById(id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        product.Update(name, color);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ProductMapper.Map(product);
    }

    public async Task<bool> DeleteProduct(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await FindUserProductById(id, cancellationToken);

        if (product is null)
        {
            return false;
        }

        dbContext.Products.Remove(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
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

    private async Task<Product?> FindUserProductById(int id, CancellationToken cancellationToken)
    {
        var userProduct = await dbContext.Products
            .Include(x => x.CreatedBy)
            .SingleOrDefaultAsync(p => p.Id == id && p.CreatedById == userService.UserId, cancellationToken);

        return userProduct;
    }
}