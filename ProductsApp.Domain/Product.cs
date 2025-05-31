namespace ProductsApp.Domain;

public class Product : BaseEntity
{
    public string Name { get; private set; }

    public string Color { get; private set; }

    public int CreatedById { get; private set; }

    public User CreatedBy { get; private set; }

    public Product(string name, string color, int createdById)
    {
        Name = name;
        Color = color;
        CreatedById = createdById;
    }
    
    public Product(string name, string color, User createdBy)
    {
        Name = name;
        Color = color;
        CreatedBy = createdBy;
    }
    
    public Product(string name, string color)
    {
        Name = name;
        Color = color;
    }
}