using mate.Interface;
using mate.Models;

namespace mate.Menus;

public class LevelsMenu : MenuBase
{
    private readonly ILevelManagerService _levelManager;

    public LevelsMenu(IGameService game, ILevelManagerService levelManager)
        : base(game)
    {
        _levelManager = levelManager;
    }

    public void Show(User user)
    {
        while (true)
        {
            int choice = SelectLevel(user);

            if (choice < 0 || choice >= _levelManager.Levels.Count)
                return;

            Level level = _levelManager.Levels[choice];

            if (!level.Start())
                continue;

            bool completed = level.Play();

            if (completed)
            {
                _levelManager.CompleteLevel(user, level);
                ShowLevelComplete(user, level);
            }
            else
            {
                ShowLevelLeft(level);
            }
        }
    }

    private int SelectLevel(User user)
    {
        IReadOnlyList<Level> levels = _levelManager.Levels;

        string[] options = new string[levels.Count + 1];

        for (int i = 0; i < levels.Count; i++)
        {
            bool done = _levelManager.IsCompleted(user, levels[i]);

            options[i] = $"{i + 1}. {levels[i].Name}{(done ? " *" : "")}";
        }

        options[levels.Count] = "0. Back";

        int selectedIndex = 0;

        string title = "------Levels------";
        string prompt = "Press Enter To Play The Selected Level";

        Game.Background();

        WriteCentered(title, Game.Height / 2 - 8, ConsoleColor.Yellow);

        DrawProgressBar(user, Game.Height / 2 - 5);

        if (levels.Count == 0)
        {
            WriteCentered(
                "No levels found! (Data/levels.json is missing)",
                Game.Height / 2,
                ConsoleColor.Red
            );
        }

        while (true)
        {
            DrawLevelMenu(options, selectedIndex, user);

            WriteCentered(prompt, Game.Height - 8, ConsoleColor.Gray);

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

                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    return levels.Count;

                default:
                    int index = GetNumber(key);

                    if (index >= 0 && index < levels.Count)
                        return index;

                    break;
            }
        }
    }

    private static int GetNumber(ConsoleKey key)
    {
        if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
            return key - ConsoleKey.D1;

        if (key >= ConsoleKey.NumPad1 && key <= ConsoleKey.NumPad9)
            return key - ConsoleKey.NumPad1;

        return -1;
    }

    private void DrawLevelMenu(string[] options, int selectedIndex, User user)
    {
        IReadOnlyList<Level> levels = _levelManager.Levels;

        for (int i = 0; i < options.Length; i++)
        {
            int x = (Game.Width - options[i].Length) / 2;
            int y = Game.Height / 2 - 1 + i;

            Console.SetCursorPosition(x, y);

            bool completed = i < levels.Count
                && _levelManager.IsCompleted(user, levels[i]);

            if (i == selectedIndex)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (completed)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.White;

            Console.Write(options[i]);
            Console.ResetColor();
        }
    }

    private void DrawProgressBar(User user, int y)
    {
        const int barLength = 26;

        int total = _levelManager.Levels.Count;
        int completed = total == 0 ? 0 : _levelManager.CountCompleted(user);

        double ratio = total == 0 ? 0 : (double)completed / total;
        int filled = (int)Math.Round(ratio * barLength);
        int percent = (int)Math.Round(ratio * 100);

        string label = $"{percent}% ({completed}/{total})";
        string bar = $"[{new string('#', filled)}{new string('-', barLength - filled)}] {label}";

        int x = Math.Max(0, (Game.Width - bar.Length) / 2);
        Console.SetCursorPosition(x, y);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write('[');

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(new string('#', filled));

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(new string('-', barLength - filled));
        Console.Write(']');

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($" {label}");

        Console.ResetColor();
    }

    private void ShowLevelComplete(User user, Level level)
    {
        Game.ProgressBar(700);

        Game.Background();

        WriteCentered("------Level Complete!------", Game.Height / 2 - 3, ConsoleColor.Green);

        WriteCentered(
            $"You completed: {level.Name}",
            Game.Height / 2 - 1,
            ConsoleColor.Yellow
        );

        DrawProgressBar(user, Game.Height / 2 + 1);

        if (user.CompleteLevelIds.Count == _levelManager.Levels.Count && _levelManager.Levels.Count > 0)
        {
            WriteCentered(
                "INCREDIBLE! You completed every level! You are a true IT Helper!",
                Game.Height / 2 + 3,
                ConsoleColor.Green
            );
        }

        WriteCentered(
            "Your progress has been saved.",
            Game.Height / 2 + 5,
            ConsoleColor.DarkGray
        );

        WriteCentered(
            "Press any key to return to the levels menu...",
            Game.Height / 2 + 7,
            ConsoleColor.Gray
        );

        Console.ReadKey(true);
    }

    private void ShowLevelLeft(Level level)
    {
        Game.Background();

        WriteCentered("------See You Soon!------", Game.Height / 2 - 2, ConsoleColor.Yellow);

        WriteCentered($"You left {level.Name}.", Game.Height / 2, ConsoleColor.White);

        WriteCentered(
            "No worries, the level will be waiting for you.",
            Game.Height / 2 + 1,
            ConsoleColor.DarkGray
        );

        WriteCentered(
            "Press any key to return to the levels menu...",
            Game.Height / 2 + 4,
            ConsoleColor.Gray
        );

        Console.ReadKey(true);
    }
}