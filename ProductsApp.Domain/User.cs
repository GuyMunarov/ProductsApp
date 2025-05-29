namespace ProductsApp.Domain;

public class User : BaseEntity
{
    public string Username { get; set; }

    public IList<Product> Products { get; set; }

    public User(string username)
    {
        Username = username;
    }

    private User()
    {
    }
}