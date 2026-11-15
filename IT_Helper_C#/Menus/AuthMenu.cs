using mate.Interface;
using mate.Models;

namespace mate.Menus;

public class AuthMenu : MenuBase
{
    private readonly IAuthService _authService;
    private readonly AdminMenu _adminMenu;

    public AuthMenu(IGameService game, IAuthService authService, AdminMenu adminMenu)
        : base(game)
    {
        _authService = authService;
        _adminMenu = adminMenu;
    }

    public User? Show()
    {
        string[] options =
        {
            "1. Login",
            "2. Register",
            "3. Admin",
            "0. Back"
        };

        while (true)
        {
            int choice = Game.SelectMenu("------Account------", options);

            switch (choice)
            {
                case 0:
                    Game.ProgressBar(700);
                    User? user = Login();

                    if (user != null)
                        return user;

                    break;

                case 1:
                    Game.ProgressBar(700);
                    User? newUser = Register();

                    if (newUser != null)
                        return newUser;

                    break;

                case 2:
                    Game.ProgressBar(700);
                    _adminMenu.Login();
                    break;

                case 3:
                case -1:
                    Game.ProgressBar(700);
                    return null;
            }
        }
    }

    private User? Login()
    {
        var (username, password) = ReadCredentials("------Login------");

        User? user = _authService.Login(username, password);

        if (user == null)
        {
            ShowMessage("Invalid username or password!", ConsoleColor.Red);
            return null;
        }

        ShowMessage($"Welcome back, {user.Username}!", ConsoleColor.Green);

        return user;
    }

    private User? Register()
    {
        var (username, password) = ReadCredentials("------Register------");

        User? user = _authService.Register(username, password, out string error);

        if (user == null)
        {
            ShowMessage(error, ConsoleColor.Red);
            return null;
        }

        ShowMessage("Registration completed!", ConsoleColor.Green);

        return user;
    }
}