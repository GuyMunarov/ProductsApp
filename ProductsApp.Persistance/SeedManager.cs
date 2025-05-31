using Microsoft.Extensions.DependencyInjection;
using ProductsApp.Domain;

namespace ProductsApp.Persistance;

public static class SeedManager
{
    public static void SeedRecords(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.EnsureCreated();

        if (!db.Users.Any())
        {
            var alice = new User("alice");
            var bob = new User("bob");
            var charlie = new User("charlie");

            alice.Products = new List<Product>
            {
                new("iPhone", "Black"),
                new("MacBook", "Silver")
            };

            bob.Products = new List<Product>
            {
                new("Headphones", "White"),
            };

            charlie.Products = new List<Product>
            {
                new("Camera", "Black"),
                new("Tripod", "Gray"),
                new("Backpack", "Blue"),
            };

            db.Users.AddRange(alice, bob, charlie);
            db.SaveChanges();
        }
    }
}