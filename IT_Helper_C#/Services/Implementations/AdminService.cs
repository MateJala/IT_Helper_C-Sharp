using IT_Helper_C_.Services.Interface;
using mate.Interface;
using mate.Models;

namespace mate.Services;

public class AdminService : IAdminService
{
    private const string AdminUsername = "admin";
    private const string AdminPassword = "password";

    private readonly IUserService _userService;

    public AdminService(IUserService userService)
    {
        _userService = userService;
    }

    public bool ValidateAdmin(string username, string password)
    {
        return username == AdminUsername && password == AdminPassword;
    }

    public List<User> GetUsers()
    {
        return _userService.GetAllUsers();
    }

    public bool DeleteUser(string username)
    {
        return _userService.DeleteUser(username);
    }
}