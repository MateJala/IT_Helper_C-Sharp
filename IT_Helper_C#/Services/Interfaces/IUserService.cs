using mate.Models;

namespace IT_Helper_C_.Services.Interface;

public interface IUserService
{

    public bool CreateUser(User user);
    public List<User> GetAllUsers();

    public User? Login(string username, string password);

    public bool ValidateUser(User user);

    public bool CompleteLevel(User user, int levelId, int totalLevels);

    public bool DeleteUser(string username);
}
