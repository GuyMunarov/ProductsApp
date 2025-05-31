using ProductsApp.Domain;
using ProductsApp.Infrastructure.Services;

namespace ProductsApp.Tests.Proxies;

internal class UserServiceProxy(User user) : IUserService
{
    public int? UserId { get; } = user.Id;

    public string? UserName { get; } = user.Username;
}