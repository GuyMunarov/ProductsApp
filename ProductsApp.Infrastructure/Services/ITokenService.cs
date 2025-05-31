namespace ProductsApp.Infrastructure.Services;

public interface ITokenService
{
    public string GenerateToken(int id, string username);
}