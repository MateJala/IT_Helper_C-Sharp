using mate.Interface;

namespace mate.Menus;

public abstract class MenuBase
{
    protected readonly IGameService Game;

    protected MenuBase(IGameService game)
    {
        Game = game;
    }

    protected void WriteCentered(string text, int y, ConsoleColor color)
    {
        Console.ForegroundColor = color;

        Console.SetCursorPosition((Game.Width - text.Length) / 2, y);

        Console.Write(text);

        Console.ResetColor();
    }

    protected void ShowMessage(string message, ConsoleColor color)
    {
        Game.ProgressBar(700);

        Game.Background();

        WriteCentered(message, Game.Height / 2, color);

        WriteCentered(
            "Press any key to continue...",
            Game.Height / 2 + 3,
            ConsoleColor.Gray
        );

        Console.ReadKey(true);
    }

    protected (string Username, string Password) ReadCredentials(string title)
    {
        Game.Background();

        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.SetCursorPosition(
            (Game.Width - title.Length) / 2,
            Game.Height / 2 - 5
        );

        Console.Write(title);

        string usernameLabel = "Username:";
        string passwordLabel = "Password:";

        int labelX = (Game.Width - usernameLabel.Length) / 2 - 12;
        int inputX = Game.Width / 2;

        int usernameY = Game.Height / 2 - 1;
        int passwordY = Game.Height / 2 + 1;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(labelX, usernameY);
        Console.Write(usernameLabel);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.SetCursorPosition(labelX, passwordY);
        Console.Write(passwordLabel);

        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(inputX, usernameY);

        Console.CursorVisible = true;
        string username = Console.ReadLine() ?? "";

        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(labelX, usernameY);
        Console.Write(usernameLabel);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(labelX, passwordY);
        Console.Write(passwordLabel);

        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(inputX, passwordY);

        string password = ReadPassword();

        Console.CursorVisible = false;
        Console.ResetColor();

        return (username, password);
    }

    private string ReadPassword()
    {
        string password = "";

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
                break;

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password[..^1];

                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

        return password;
    }
}