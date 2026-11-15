using mate.Interface;

namespace mate.Models.Levels;

public class CodeLevel : Level
{
    private readonly List<string> _lines;
    private readonly int _buggyLine;
    private readonly string _fix;

    public CodeLevel(
        IGameService game,
        int id,
        string name,
        string description,
        List<string> lines,
        int buggyLine,
        string fix
    )
        : base(game, id, name, description)
    {
        _lines = lines;
        _buggyLine = buggyLine;
        _fix = fix;
    }

    public override bool Play()
    {
        int selectedIndex = 0;
        int wrongPicks = 0;

        DrawScreen(wrongPicks);

        while (true)
        {
            DrawCode(selectedIndex);

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.DownArrow:
                    selectedIndex++;
                    if (selectedIndex >= _lines.Count)
                        selectedIndex = 0;
                    break;

                case ConsoleKey.UpArrow:
                    selectedIndex--;
                    if (selectedIndex < 0)
                        selectedIndex = _lines.Count - 1;
                    break;

                case ConsoleKey.Enter:
                    if (selectedIndex == _buggyLine)
                    {
                        ShowSuccess();
                        return true;
                    }

                    wrongPicks++;
                    ShowWrongPick(wrongPicks);
                    DrawScreen(wrongPicks);
                    break;

                case ConsoleKey.Escape:
                    return false;
            }
        }
    }

    private void DrawScreen(int wrongPicks)
    {
        Game.Background();

        WriteCentered($"------{Name}------", Game.Height / 2 - 11, ConsoleColor.Yellow);

        WriteCentered(
            "Find the line with the mistake!",
            Game.Height / 2 - 9,
            ConsoleColor.White
        );

        if (wrongPicks > 0)
        {
            WriteCentered(
                $"Wrong picks: {wrongPicks}",
                Game.Height / 2 - 8,
                ConsoleColor.DarkYellow
            );
        }

        WriteCentered(
            "[Up/Down] Move   [Enter] Flag The Line   [Esc] Leave",
            Game.Height - 7,
            ConsoleColor.Gray
        );
    }

    private void DrawCode(int selectedIndex)
    {
        int numberWidth = _lines.Count.ToString().Length;

        int longest = _lines.Max(x => x.Length);

        int x = (Game.Width - (longest + numberWidth + 5)) / 2;
        int y = Game.Height / 2 - 6;

        for (int i = 0; i < _lines.Count; i++)
        {
            Console.SetCursorPosition(x, y + i);

            if (i == selectedIndex)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.White;

            string marker = i == selectedIndex ? "> " : "  ";

            Console.Write(
                $"{marker}{(i + 1).ToString().PadLeft(numberWidth)}  {_lines[i]}"
            );

            Console.ResetColor();
        }
    }

    private void ShowWrongPick(int wrongPicks)
    {
        WriteCentered(
            "Nope, that line looks fine. Look more carefully...",
            Game.Height - 5,
            ConsoleColor.Red
        );

        WriteCentered(
            "Press any key to keep looking...",
            Game.Height - 4,
            ConsoleColor.Gray
        );

        Console.ReadKey(true);
    }

    private void ShowSuccess()
    {
        Game.Background();

        WriteCentered("------You Found The Bug!------", Game.Height / 2 - 4, ConsoleColor.Green);

        WriteCentered(
            $"{Name} is now fixed!",
            Game.Height / 2 - 2,
            ConsoleColor.Yellow
        );

        List<string> lines = WrapText($"The fix: {_fix}", 76);

        int y = Game.Height / 2 + 1;

        foreach (string line in lines)
        {
            WriteCentered(line, y, ConsoleColor.White);
            y++;
        }

        WriteCentered(
            "Press any key to continue...",
            Game.Height / 2 + 5,
            ConsoleColor.Gray
        );

        Console.ReadKey(true);
    }
}
