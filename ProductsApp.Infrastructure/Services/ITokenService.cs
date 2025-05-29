namespace ProductsApp.Infrastructure.Services;

public interface ITokenService
{
    public string GenerateToken(string username);
}