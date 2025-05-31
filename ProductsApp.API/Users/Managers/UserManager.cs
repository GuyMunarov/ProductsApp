using Microsoft.EntityFrameworkCore;
using ProductsApp.API.Users.Models;
using ProductsApp.Domain;
using ProductsApp.Infrastructure.Services;
using ProductsApp.Persistance;

namespace ProductsApp.API.Users.Managers;

internal class UserManager(AppDbContext dbContext, ITokenService tokenService) : IUserManager
{
    public async Task<TokenResponse?> Login(string username, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Username == username, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var token = tokenService.GenerateToken(user.Id, user.Username);

        return new TokenResponse(token);
    }

    public async Task<TokenResponse?> Register(string username, CancellationToken cancellationToken)
    {
        var user = new User(username);
        
        var registeredUser = await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var token = tokenService.GenerateToken(registeredUser.Entity.Id, registeredUser.Entity.Username);

        return new TokenResponse(token);
    }
}