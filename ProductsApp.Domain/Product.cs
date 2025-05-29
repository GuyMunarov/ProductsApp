namespace ProductsApp.Domain;

public class Product : BaseEntity
{
    public required string Name { get; set; }

    public required string Color { get; set; }

    public int CreatedById { get; set; }

    public User CreatedBy { get; set; }
}