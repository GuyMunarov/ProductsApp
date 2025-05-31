using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsApp.Domain;

namespace ProductsApp.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Username)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasIndex(p => p.Username)
            .IsUnique();
        
        builder.HasMany(x => x.Products)
            .WithOne(x => x.CreatedBy)
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Cascade);
    }
}