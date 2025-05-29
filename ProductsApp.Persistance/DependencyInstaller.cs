using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsApp.Persistance;

public static class DependencyInstaller
{
    public static IServiceCollection InstallPersistance(this IServiceCollection services)
        => services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));
}