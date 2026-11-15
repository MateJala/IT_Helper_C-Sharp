using IT_Helper_C_.Services.Interface;
using IT_Helper_C_.Services.Services;
using mate.Interface;
using mate.Menus;
using mate.Services;

public class Program
{
    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Thread.Sleep(1800);

        IGameService gameService = new GameService(true);
        IUserService userService = new UserService();
        IAdminService adminService = new AdminService(userService);
        IAuthService authService = new AuthService(userService);
        ILevelManagerService levelManager = new LevelManagerService(userService, gameService);

        AdminMenu adminMenu = new AdminMenu(gameService, adminService);
        AuthMenu authMenu = new AuthMenu(gameService, authService, adminMenu);
        LevelsMenu levelsMenu = new LevelsMenu(gameService, levelManager);
        MainMenu mainMenu = new MainMenu(gameService, authMenu, levelsMenu);

        mainMenu.Run();

        Console.ResetColor();
        Console.CursorVisible = true;
    }
}