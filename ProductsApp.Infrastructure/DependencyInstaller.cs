using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductsApp.Infrastructure.Services;

namespace ProductsApp.Infrastructure;

public static class DependencyInstaller
{
    public static IServiceCollection InstallInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
                configuration.GetSection("Jwt"))
            .AddHttpContextAccessor()
            .AddScoped<ITokenService, JwtTokenService>()
            .AddScoped<IUserService, UserService>();

        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Secret)
                    )
                };
            });

        services.AddAuthorization();

        return services;
    }
}