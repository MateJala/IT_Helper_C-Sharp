using mate.Models;

namespace mate.Interface;

public interface IAuthService
{
    User? Login(string username, string password);
    User? Register(string username, string password, out string error);
}