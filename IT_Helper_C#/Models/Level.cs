using mate.Interface;

namespace mate.Models;

public abstract class Level
{
    protected readonly IGameService Game;

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }

    protected Level(IGameService game, int id, string name, string description)
    {
        Game = game;
        Id = id;
        Name = name;
        Description = description;
    }

    public bool Start()
    {
        Game.Background();

        WriteCentered($"------{Name}------", Game.Height / 2 - 9, ConsoleColor.Yellow);

        List<string> lines = WrapText(Description, 76);
        int y = Game.Height / 2 - 6;

        foreach (string line in lines)
        {
            WriteCentered(line, y, ConsoleColor.White);
            y++;
        }

        WriteCentered(
            "Press any key to start the level...",
            Game.Height / 2 + 8,
            ConsoleColor.Green
        );

        WriteCentered(
            "[Esc] Back to the levels menu",
            Game.Height / 2 + 10,
            ConsoleColor.DarkGray
        );

        return Console.ReadKey(true).Key != ConsoleKey.Escape;
    }

    public abstract bool Play();

    protected void WriteCentered(string text, int y, ConsoleColor color)
    {
        Console.ForegroundColor = color;

        Console.SetCursorPosition((Game.Width - text.Length) / 2, y);

        Console.Write(text);

        Console.ResetColor();
    }

    protected static List<string> WrapText(string text, int maxWidth)
    {
        List<string> lines = new List<string>();

        string current = "";

        foreach (string word in text.Split(' '))
        {
            if (current.Length == 0)
                current = word;
            else if (current.Length + 1 + word.Length <= maxWidth)
                current += $" {word}";
            else
            {
                lines.Add(current);
                current = word;
            }
        }

        if (current.Length > 0)
            lines.Add(current);

        return lines;
    }
}
