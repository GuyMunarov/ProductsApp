using Microsoft.EntityFrameworkCore;
using ProductsApp.API.Users.Managers;
using ProductsApp.Domain;
using ProductsApp.Tests.Proxies;
using static ProductsApp.Tests.TestDbUtils;

namespace ProductsApp.Tests;

public class UsersManagerTests
{
    [Fact]
    public async Task LoginUser_Works()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        await context.Users.AddAsync(user);

        await context.SaveChangesAsync();

        var tokenService = new TokenServiceProxy();

        var manager = new UserManager(context, tokenService);

        var result = await manager.Login("alice", CancellationToken.None);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task LoginUser_DoesntWork()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        await context.Users.AddAsync(user);

        await context.SaveChangesAsync();

        var tokenService = new TokenServiceProxy();

        var manager = new UserManager(context, tokenService);

        var result = await manager.Login("tom", CancellationToken.None);

        Assert.Null(result);
    }
    
    [Fact]
    public async Task RegisterUser_Works()
    {
        await using var context = CreateInMemoryDbContext();

        var tokenService = new TokenServiceProxy();

        var manager = new UserManager(context, tokenService);

        var result = await manager.Register("tom", CancellationToken.None);

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task RegisterUser_DoesntWork()
    {
        await using var context = CreateInMemoryDbContext();

        var user = new User("alice");

        await context.Users.AddAsync(user);
        
        await context.SaveChangesAsync();
        
        var tokenService = new TokenServiceProxy();

        var manager = new UserManager(context, tokenService);
        
        await Assert.ThrowsAsync<DbUpdateException>(() => manager.Register("alice", CancellationToken.None));
    }
}