﻿using Microsoft.EntityFrameworkCore;
using ProductsApp.Domain;

namespace ProductsApp.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }

    public DbSet<Product> Products { get; init; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}