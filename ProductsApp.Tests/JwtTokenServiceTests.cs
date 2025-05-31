using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using ProductsApp.Infrastructure;
using ProductsApp.Infrastructure.Services;

namespace ProductsApp.Tests;

public class JwtTokenServiceTests
{
    private readonly JwtOptions _options = new()
    {
        Secret = "123412asdasdasdasfgsgdffgsfdsfdsfdsfsdfsdfdsfdsf",
        Issuer = "productsApp"
    };

    [Fact]
    public void GenerateToken_ShouldContainUsernameClaim_AndIssuer()
    {
        var optionsMock = Options.Create(_options);
        var jwtService = new JwtTokenService(optionsMock);
        
        var username = "testuser";
        var id = 100;
        
        var token = jwtService.GenerateToken(id, username);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal(_options.Issuer, jwt.Issuer);

        var usernameClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        Assert.NotNull(usernameClaim);
        Assert.NotNull(idClaim);
            
        Assert.Equal(username, usernameClaim!.Value);
        Assert.Equal(id.ToString(), idClaim!.Value);
        
    }
}