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
                new() { Name = "iPhone", Color = "Black", CreatedBy = alice },
                new() { Name = "MacBook", Color = "Silver", CreatedBy = alice },
            };

            bob.Products = new List<Product>
            {
                new() { Name = "Headphones", Color = "White", CreatedBy = bob },
            };

            charlie.Products = new List<Product>
            {
                new() { Name = "Camera", Color = "Black", CreatedBy = charlie },
                new() { Name = "Tripod", Color = "Gray", CreatedBy = charlie },
                new() { Name = "Backpack", Color = "Blue", CreatedBy = charlie },
            };

            db.Users.AddRange(alice, bob, charlie);
            db.SaveChanges();
        }
    }
}