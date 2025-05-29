using ProductsApp.Domain;

namespace ProductsApp.Persistance.Abstractation;

public interface IUsersRepository
{
    User? GetUser(string username);
}