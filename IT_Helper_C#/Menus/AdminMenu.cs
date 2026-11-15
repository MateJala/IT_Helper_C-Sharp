using mate.Interface;
using mate.Models;

namespace mate.Menus;

public class AdminMenu : MenuBase
{
    private readonly IAdminService _adminService;

    public AdminMenu(IGameService game, IAdminService adminService)
        : base(game)
    {
        _adminService = adminService;
    }

    public void Login()
    {
        var (username, password) = ReadCredentials("------Admin Login------");

        if (!_adminService.ValidateAdmin(username, password))
        {
            ShowMessage("Invalid admin credentials!", ConsoleColor.Red);
            return;
        }

        ShowMessage("Welcome, Administrator!", ConsoleColor.Green);

        Show();
    }

    private void Show()
    {
        string[] options =
        {
            "1. View Users",
            "2. Delete User",
            "0. Back"
        };

        while (true)
        {
            int choice = Game.SelectMenu("------Admin------", options);

            switch (choice)
            {
                case 0:
                    ViewUsers();
                    break;

                case 1:
                    DeleteUser();
                    break;

                case 2:
                case -1:
                    return;
            }
        }
    }

    private void ViewUsers()
    {
        Game.ProgressBar(700);

        Game.Background();

        WriteCentered("------Users------", Game.Height / 2 - 8, ConsoleColor.Yellow);

        List<User> users = _adminService.GetUsers();

        if (users.Count == 0)
        {
            WriteCentered("No users found!", Game.Height / 2, ConsoleColor.Red);
        }
        else
        {
            int y = Game.Height / 2 - 6;

            foreach (User user in users)
            {
                WriteCentered(
                    $"{user.Id} - {user.Username} - {Math.Round(user.CompleteLevelIds.Count * 100.0 / 8.0)}% completed",
                    y,
                    ConsoleColor.White
                );

                y++;
            }
        }

        WriteCentered(
            "Press any key to continue...",
            Game.Height / 2 + 8,
            ConsoleColor.Gray
        );

        Console.ReadKey(true);
    }

    private void DeleteUser()
    {
        Game.ProgressBar(700);

        Game.Background();

        WriteCentered("------Delete User------", Game.Height / 2 - 8, ConsoleColor.Yellow);

        List<User> users = _adminService.GetUsers();

        if (users.Count == 0)
        {
            WriteCentered(
                "There are no users to delete!",
                Game.Height / 2,
                ConsoleColor.Red
            );

            WriteCentered(
                "Press any key to continue...",
                Game.Height / 2 + 3,
                ConsoleColor.Gray
            );

            Console.ReadKey(true);
            return;
        }

        int y = Game.Height / 2 - 6;

        foreach (User user in users)
        {
            WriteCentered($"{user.Id}. {user.Username}", y, ConsoleColor.White);

            y++;
        }

        WriteCentered(
            "Enter the username of the account to delete:",
            Game.Height / 2 + 8,
            ConsoleColor.Gray
        );

        Console.CursorVisible = true;

        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(Game.Width / 2, Game.Height / 2 + 9);

        string username = Console.ReadLine() ?? "";

        Console.CursorVisible = false;
        Console.ResetColor();

        if (string.IsNullOrWhiteSpace(username))
        {
            ShowMessage("No username was entered!", ConsoleColor.Red);
            return;
        }

        WriteCentered(
            $"Are you sure you want to delete '{username}'? [Y/N]",
            Game.Height / 2 + 11,
            ConsoleColor.Yellow
        );

        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Y)
            {
                bool deleted = _adminService.DeleteUser(username);

                ShowMessage(
                    deleted
                        ? $"User '{username}' was deleted!"
                        : $"User '{username}' was not found!",
                    deleted ? ConsoleColor.Green : ConsoleColor.Red
                );

                return;
            }

            if (key == ConsoleKey.N)
            {
                ShowMessage("Deletion cancelled.", ConsoleColor.DarkYellow);
                return;
            }
        }
    }
}