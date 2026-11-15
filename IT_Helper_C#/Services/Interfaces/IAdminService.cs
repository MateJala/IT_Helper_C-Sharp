using mate.Models;

namespace mate.Interface;

public interface IAdminService
{
    bool ValidateAdmin(string username, string password);
    List<User> GetUsers();
    bool DeleteUser(string username);
}