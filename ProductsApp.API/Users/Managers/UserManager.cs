using Microsoft.EntityFrameworkCore;
using ProductsApp.API.Users.Models;
using ProductsApp.Infrastructure.Services;
using ProductsApp.Persistance;

namespace ProductsApp.API.Users.Managers;

internal class UserManager(AppDbContext dbContext, ITokenService tokenService) : IUserManager
{
    public async Task<TokenResponse?> Login(string username, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Username == username, cancellationToken);

        if (user == null)
        {
            return null;
        }

        var token = tokenService.GenerateToken(user.Username);

        return new TokenResponse(token);
    }
}