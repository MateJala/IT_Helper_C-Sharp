using IT_Helper_C_.Services.Interface;
using mate.Enum;
using mate.Interface;
using mate.Models;

namespace mate.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }

    public User? Login(string username, string password)
    {
        return _userService.Login(username, password);
    }

    public User? Register(string username, string password, out string error)
    {
        error = "";

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            error = "Username and password cannot be empty!";
            return null;
        }

        User user = new User(
            0,
            username,
            password,
            Role.User,
            DateTime.Now.ToString()
        );

        if (!_userService.CreateUser(user))
        {
            error = "Registration failed! Username may already exist.";
            return null;
        }

        return user;
    }
}