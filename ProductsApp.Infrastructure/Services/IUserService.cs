namespace ProductsApp.Infrastructure.Services;

public interface IUserService
{
    int? UserId { get; }
    
    string? UserName { get; }
}