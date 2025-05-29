using Microsoft.AspNetCore.Mvc;
using ProductsApp.API.Abstractation;
using ProductsApp.API.Users.Managers;
using ProductsApp.API.Users.Models;

namespace ProductsApp.API.Users;

public class UsersController(IUserManager userManager) : BaseController
{
    [HttpGet("login")]
    public async Task<IActionResult> Get([FromQuery] LoginRequest request, CancellationToken cancellationToken)
    {
        var tokenResponse = await userManager.Login(request.Username, cancellationToken);

        if (tokenResponse == null)
        {
            return Unauthorized();
        }

        return Ok(tokenResponse);
    }
}