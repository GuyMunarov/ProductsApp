using ProductsApp.Domain;
using ProductsApp.Infrastructure.Services;

namespace ProductsApp.Tests.Proxies;

internal class TokenServiceProxy : ITokenService
{
    public string GenerateToken(int id, string username)
    {
        return "proxyToken";
    }
}