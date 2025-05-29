using ProductsApp.API.Users.Models;

namespace ProductsApp.API.Users.Managers;

public interface IUserManager
{
    Task<TokenResponse?> Login(string username, CancellationToken cancellationToken);
}