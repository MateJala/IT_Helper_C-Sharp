using mate.Interface;
using mate.Models;

namespace mate.Menus;

public class MainMenu : MenuBase
{
    private readonly AuthMenu _authMenu;
    private readonly LevelsMenu _levelsMenu;

    public MainMenu(IGameService game, AuthMenu authMenu, LevelsMenu levelsMenu)
        : base(game)
    {
        _authMenu = authMenu;
        _levelsMenu = levelsMenu;
    }

    public void Run()
    {
        string[] menuArray =
        {
            "1. Enter Game",
            "2. Credits",
            "0. Exit"
        };

        while (Game.IsRunning)
        {
            int choice = Game.SelectMenu("------Menu------", menuArray);

            switch (choice)
            {
                case 0:
                    Game.ProgressBar(700);
                    User? user = _authMenu.Show();

                    if (user != null)
                    {
                        Game.Background();

                        WriteCentered(
                            $"Welcome, {user.Username}!",
                            Game.Height / 2,
                            ConsoleColor.Green
                        );

                        Console.SetCursorPosition(
                            Game.Width / 2 - 14,
                            Game.Height / 2 + 3
                        );

                        Console.Write("Press any key to continue...");
                        Console.ReadKey(true);

                        _levelsMenu.Show(user);
                    }

                    break;

                case 1:
                    ShowCredits();
                    break;

                case 2:
                case -1:
                    ShowExit();
                    Game.IsRunning = false;
                    break;
            }
        }
    }

    private void ShowCredits()
    {
        Game.ProgressBar();

        Game.Background();

        string[] credits =
        {
            "------Credits------",
            "Team MVP: Mate Jalagonia",
            "Game Idea: Mate Jalagonia",
            "Game Design: Mate Jalagonia",
            "Game Code: Mate Jalagonia",
            "Hope & Dreams: Mate Jalagonia",
            "Teacher: Liza Khulelidze"
        };

        Console.ForegroundColor = ConsoleColor.Yellow;

        for (int i = 0; i < credits.Length; i++)
        {
            Console.SetCursorPosition(
                (Game.Width - credits[i].Length) / 2,
                Game.Height / 2 - 3 + i
            );

            Console.Write(credits[i]);
        }

        Console.ResetColor();

        Console.SetCursorPosition(
            Game.Width / 2 - 12,
            Game.Height / 2 + 6
        );

        Console.Write("Press any key to return");

        Console.ReadKey(true);
    }

    private void ShowExit()
    {
        Game.ProgressBar(700);

        Game.Background();

        string[] exit =
        {
            "------Exiting!------",
            "Hope you Liked The Game!",
            "Always Be Welcome To Come Back!"
        };

        Console.ForegroundColor = ConsoleColor.Red;

        for (int i = 0; i < exit.Length; i++)
        {
            Console.SetCursorPosition(
                (Game.Width - exit[i].Length) / 2,
                Game.Height / 2 - 2 + i
            );

            Console.Write(exit[i]);
        }

        Console.ResetColor();

        Console.SetCursorPosition(0, 0);
    }
}