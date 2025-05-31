using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.API.Abstractation;
using ProductsApp.API.Users.Managers;
using ProductsApp.API.Users.Models;

namespace ProductsApp.API.Users;

public class UsersController(IUserManager userManager, IValidator<RegisterRequest> registerValidator) : BaseController
{
    [HttpGet("login")]
    public async Task<IActionResult> Get([FromQuery] LoginRequest request, CancellationToken cancellationToken)
    {
        var tokenResponse = await userManager.Login(request.Username, cancellationToken);

        if (tokenResponse is null)
        {
            return Unauthorized();
        }

        return Ok(tokenResponse);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Post([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await registerValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        var tokenResponse = await userManager.Register(request.Username, cancellationToken);

        if (tokenResponse is null)
        {
            return Unauthorized();
        }

        return Ok(tokenResponse);
    }
}