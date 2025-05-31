using Microsoft.EntityFrameworkCore;
using ProductsApp.Persistance;

namespace ProductsApp.Tests;

public static class TestDbUtils
{
    public static AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }
}