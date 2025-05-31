using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ProductsApp.Infrastructure.Services;

internal class UserService : IUserService
{
    public int? UserId { get; }

    public string? UserName { get; }

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated ?? false)
        {
            UserId = int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId)
                ? userId
                : null;
            UserName = user.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}