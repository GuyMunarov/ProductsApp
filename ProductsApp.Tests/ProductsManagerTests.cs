using Microsoft.EntityFrameworkCore;
using ProductsApp.API.Products.Managers;
using ProductsApp.Domain;
using ProductsApp.Persistance;

namespace ProductsApp.Tests;

public class ProductsManagerTests
{
    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Filename=:memory:") // SQLite in-memory
            .Options;

        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }

    [Fact]
    public async Task QueryProducts_FiltersByNameAndColor()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");
        context.Users.Add(user);
        context.Products.AddRange(
            new Product { Name = "iPhone", Color = "Black", CreatedBy = user },
            new Product { Name = "MacBook", Color = "Silver", CreatedBy = user }
        );
        await context.SaveChangesAsync();

        var manager = new ProductsManager(context);

        var result = await manager.QueryProducts(name: "iPhone", color: null, createdBy: null, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("iPhone", result.First().Name);
    }
    
    [Fact]
    public async Task QueryProducts_FiltersByCreatedBy()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");
        var user2 = new User("Tom");
            
        context.Users.Add(user);
        context.Products.AddRange(
            new Product { Name = "iPhone", Color = "Black", CreatedBy = user },
            new Product { Name = "MacBook", Color = "Silver", CreatedBy = user2 }
        );
        await context.SaveChangesAsync();

        var manager = new ProductsManager(context);

        var result = await manager.QueryProducts(name: null, color: null, createdBy: user2.Username, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(user2.Username, result.First().CreatedBy);
    }
    
    [Fact]
    public async Task QueryProducts_GetAll()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");
            
        context.Users.Add(user);
        context.Products.AddRange(
            new Product { Name = "iPhone", Color = "Black", CreatedBy = user },
            new Product { Name = "iPhone2", Color = "Black", CreatedBy = user },
            new Product { Name = "iPhone3", Color = "Black", CreatedBy = user },
            new Product { Name = "iPhone4", Color = "Black", CreatedBy = user },
            new Product { Name = "iPhone5", Color = "Black", CreatedBy = user },
            new Product { Name = "iPhone6", Color = "Black", CreatedBy = user }
            );
        await context.SaveChangesAsync();

        var manager = new ProductsManager(context);

        var result = await manager.QueryProducts(name: null, color: null, createdBy: null, CancellationToken.None);

        Assert.Equal(context.Products.Count(), result.Count);
    }
    
    [Fact]
    public async Task QueryProducts_GetByIdFound()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");
            
        context.Users.Add(user);
        context.Products.AddRange(
            new Product { Name = "iPhone", Color = "Black", CreatedBy = user }
        );
        await context.SaveChangesAsync();

        var manager = new ProductsManager(context);

        var id = context.Products.First().Id;

        var result = await manager.GetById(id, CancellationToken.None);

        Assert.Equal("iPhone", result!.Name);
    }
    
    [Fact]
    public async Task QueryProducts_GetByIdNotFound()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");
            
        context.Users.Add(user);
        context.Products.AddRange(
            new Product { Name = "iPhone", Color = "Black", CreatedBy = user }
        );
        await context.SaveChangesAsync();

        var manager = new ProductsManager(context);

        var id = context.Products.First().Id + 1;

        var result = await manager.GetById(id, CancellationToken.None);

        Assert.Null(result);
    }
}