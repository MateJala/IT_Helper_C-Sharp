using mate.Interface;

namespace mate.Services;

public class GameService : IGameService
{
    public int Width { get; } = Console.WindowWidth;
    public int Height { get; } = Console.WindowHeight;
    public bool IsRunning { get; set; }

    public GameService(bool isRunning = true)
    {
        IsRunning = isRunning;
    }

    public void Background()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Yellow;

        for (int x = 0; x < Width; x++)
        {
            Console.SetCursorPosition(x, 0);
            Console.Write("#");

            Console.SetCursorPosition(x, Height - 1);
            Console.Write("#");

            Console.ForegroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(x, 1);
            Console.Write("-");

            Console.SetCursorPosition(x, Height - 2);
            Console.Write("_");

            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        for (int y = 1; y < Height; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write("#");

            Console.SetCursorPosition(Width - 1, y);
            Console.Write("#");
        }

        Console.ForegroundColor = ConsoleColor.Red;

        Console.SetCursorPosition(1, 1);
        Console.Write("$#/");

        Console.SetCursorPosition(1, 2);
        Console.Write("#/");

        Console.SetCursorPosition(1, 3);
        Console.Write("/");

        Console.SetCursorPosition(Width - 4, 1);
        Console.Write("\\#$");

        Console.SetCursorPosition(Width - 3, 2);
        Console.Write("\\#");

        Console.SetCursorPosition(Width - 2, 3);
        Console.Write("\\");

        Console.SetCursorPosition(1, Height - 4);
        Console.Write("\\");

        Console.SetCursorPosition(1, Height - 3);
        Console.Write("#\\");

        Console.SetCursorPosition(1, Height - 2);
        Console.Write("$#\\");

        Console.SetCursorPosition(Width - 2, Height - 4);
        Console.Write("/");

        Console.SetCursorPosition(Width - 3, Height - 3);
        Console.Write("/#");

        Console.SetCursorPosition(Width - 4, Height - 2);
        Console.Write("/#$");

        for (int y = 4; y < Height - 4; y++)
        {
            Console.SetCursorPosition(1, y);
            Console.Write("|");

            Console.SetCursorPosition(Width - 2, y);
            Console.Write("|");
        }

        Console.SetCursorPosition(Width / 2 - 2, 1);
        Console.Write("\\===/");

        Console.SetCursorPosition(Width / 2 - 2, Height - 2);
        Console.Write("/===\\");

        Console.ResetColor();

        Console.CursorVisible = false;
        Console.SetCursorPosition(Width / 2, Height / 2);
    }

    public void DrawMenu(string[] menuArray, int selectedIndex)
    {
        for (int i = 0; i < menuArray.Length; i++)
        {
            int x = (Width - menuArray[i].Length) / 2;
            int y = Height / 2 - 1 + i;

            Console.SetCursorPosition(x, y);

            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write(menuArray[i]);
            Console.ResetColor();
        }
    }

    public void ProgressBar(int time = 1500)
    {
        Console.SetCursorPosition(Width / 2 - 14, Height - 6);

        Console.Write("[--------------------------]");

        for (int i = 0; i < 26; i++)
        {
            Console.SetCursorPosition(Width / 2 - 13 + i, Height - 6);

            Console.Write('#');

            Thread.Sleep(time / 26);
        }
    }

    public int SelectMenu(string title, string[] options)
    {
        Background();

        int selectedIndex = 0;

        string prompt = "Press Enter To Confirm Your Choice";

        Console.ForegroundColor = ConsoleColor.White;

        Console.SetCursorPosition(
            (Width - title.Length) / 2,
            Height / 2 - 3
        );

        Console.Write(title);

        while (true)
        {
            DrawMenu(options, selectedIndex);

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.SetCursorPosition(
                (Width - prompt.Length) / 2,
                Height / 2 + 4
            );

            Console.Write(prompt);

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.DownArrow:
                    selectedIndex++;

                    if (selectedIndex >= options.Length)
                        selectedIndex = 0;

                    break;

                case ConsoleKey.UpArrow:
                    selectedIndex--;

                    if (selectedIndex < 0)
                        selectedIndex = options.Length - 1;

                    break;

                case ConsoleKey.Enter:
                    return selectedIndex;

                case ConsoleKey.Escape:
                    return -1;

                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return 0;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    if (options.Length > 1)
                        return 1;
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    if (options.Length > 2)
                        return 2;
                    break;

                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    if (options.Length > 0)
                        return options.Length - 1;
                    break;
            }
        }
    }
}