using ProductsApp.API.Products.Managers;
using ProductsApp.Domain;
using ProductsApp.Tests.Proxies;
using static ProductsApp.Tests.TestDbUtils;

namespace ProductsApp.Tests;

public class ProductsManagerTests
{
    [Fact]
    public async Task QueryProducts_FiltersByNameAndColor()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        var userEntity = await context.Users.AddAsync(user);
        
        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity.Entity);

        var manager = new ProductsManager(context, userService);

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

        var userEntity = await context.Users.AddAsync(user);
        await context.Users.AddAsync(user2);

        await context.Products.AddRangeAsync(
            new Product("iPhone", "Black", user),
            new Product("MacBook", "Silver", user2)
        );

        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity.Entity);
        var manager = new ProductsManager(context, userService);

        var result =
            await manager.QueryProducts(name: null, color: null, createdBy: user2.Username, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(user2.Username, result.First().CreatedBy);
    }

    [Fact]
    public async Task QueryProducts_GetAll()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        var userEntity = await context.Users.AddAsync(user);

        await context.Products.AddRangeAsync(
            new Product("iPhone", "Black", user),
            new Product("iPhone2", "Black", user),
            new Product("iPhone3", "Black", user),
            new Product("iPhone4", "Black", user),
            new Product("iPhone5", "Black", user),
            new Product("iPhone6", "Black", user)
        );

        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity.Entity);
        var manager = new ProductsManager(context, userService);

        var result = await manager.QueryProducts(name: null, color: null, createdBy: null, CancellationToken.None);

        Assert.Equal(context.Products.Count(), result.Count);
    }

    [Fact]
    public async Task QueryProducts_GetByIdFound()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        var userEntity = await context.Users.AddAsync(user);

        await context.Products.AddAsync(new Product("iPhone", "Black", user));

        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity.Entity);
        var manager = new ProductsManager(context, userService);

        var id = context.Products.First().Id;

        var result = await manager.GetById(id, CancellationToken.None);

        Assert.Equal("iPhone", result!.Name);
    }

    [Fact]
    public async Task QueryProducts_GetByIdNotFound()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        var userEntity = await context.Users.AddAsync(user);

        await context.Products.AddAsync(new Product("iPhone", "Black", user));

        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity.Entity);
        var manager = new ProductsManager(context, userService);

        var id = context.Products.First().Id + 1;

        var result = await manager.GetById(id, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProduct()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        var userEntity = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity.Entity);
        var manager = new ProductsManager(context, userService);

        var result = await manager.CreateProduct("iPhone", "Black", CancellationToken.None);

        Assert.Equal("iPhone", result.Name);
        Assert.Equal("Black", result.Color);
        Assert.Equal("alice", result.CreatedBy);
    }

    [Fact]
    public async Task UpdateProduct_Works()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        var userEntity = await context.Users.AddAsync(user);
        var productEntity = await context.Products.AddAsync(new Product("iPhone", "Black", user));
        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity.Entity);
        var manager = new ProductsManager(context, userService);

        var result = await manager.UpdateProduct(productEntity.Entity.Id, "Macbook", "Silver", CancellationToken.None);

        Assert.Equal("Macbook", result!.Name);
        Assert.Equal("Silver", result.Color);
    }

    [Fact]
    public async Task UpdateProduct_IncorrectUser()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");
        var user1 = new User("bob");

        var userEntity1 = await context.Users.AddAsync(user1);

        var productEntity = await context.Products.AddAsync(new Product("iPhone", "Black", user));
        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity1.Entity);
        var manager = new ProductsManager(context, userService);

        var result = await manager.UpdateProduct(productEntity.Entity.Id, "Macbook", "Silver", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteProduct_Works()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        var userEntity = await context.Users.AddAsync(user);

        var productEntity = await context.Products.AddAsync(new Product("iPhone", "Black", user));
        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity.Entity);
        var manager = new ProductsManager(context, userService);

        var result = await manager.DeleteProduct(productEntity.Entity.Id, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteProduct_IncorrectUser()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");
        var user1 = new User("bob");

        var userEntity1 = await context.Users.AddAsync(user1);

        var productEntity = await context.Products.AddAsync(new Product("iPhone", "Black", user));
        await context.SaveChangesAsync();

        var userService = new UserServiceProxy(userEntity1.Entity);
        var manager = new ProductsManager(context, userService);

        var result = await manager.DeleteProduct(productEntity.Entity.Id, CancellationToken.None);

        Assert.False(result);
    }
}