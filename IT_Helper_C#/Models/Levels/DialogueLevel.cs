using mate.Interface;

namespace mate.Models.Levels;

public class DialogueLevel : Level
{
    private readonly string _character;
    private readonly List<DialogueStep> _steps;

    public DialogueLevel(
        IGameService game,
        int id,
        string name,
        string description,
        string character,
        List<DialogueStep> steps
    )
        : base(game, id, name, description)
    {
        _character = character;
        _steps = steps;
    }

    public override bool Play()
    {
        foreach (DialogueStep step in _steps)
        {
            bool solved = false;

            while (!solved)
            {
                int choice = SelectAnswer(step);

                if (choice < 0)
                    return false;

                DialogueOption option = step.Options[choice];

                solved = option.Correct;

                ShowReply(option);
            }
        }

        return true;
    }

    private int SelectAnswer(DialogueStep step)
    {
        Game.Background();

        string[] options = step.Options
            .Select(x => x.Text)
            .ToArray();

        int selectedIndex = 0;

        string prompt = "[Up/Down] Choose   [Enter] Answer   [Esc] Leave";

        WriteCentered($"------{Name}------", Game.Height / 2 - 10, ConsoleColor.Yellow);

        WriteCentered($"{_character} says:", Game.Height / 2 - 8, ConsoleColor.Cyan);

        List<string> npcLines = WrapText(step.Npc, 76);

        int y = Game.Height / 2 - 6;

        foreach (string line in npcLines)
        {
            WriteCentered(line, y, ConsoleColor.White);
            y++;
        }

        while (true)
        {
            DrawAnswers(options, selectedIndex, Game.Height / 2 + 1);

            WriteCentered(prompt, Game.Height / 2 + 6, ConsoleColor.Gray);

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
                    if (options.Length > 0)
                        return 0;
                    break;

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
            }
        }
    }

    private void DrawAnswers(string[] options, int selectedIndex, int startY)
    {
        for (int i = 0; i < options.Length; i++)
        {
            int x = (Game.Width - options[i].Length) / 2;

            Console.SetCursorPosition(x, startY + i);

            if (i == selectedIndex)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.White;

            Console.Write(options[i]);
            Console.ResetColor();
        }
    }

    private void ShowReply(DialogueOption option)
    {
        Game.Background();

        ConsoleColor replyColor = option.Correct
            ? ConsoleColor.Green
            : ConsoleColor.Red;

        WriteCentered($"{_character} replies:", Game.Height / 2 - 6, ConsoleColor.Cyan);

        List<string> lines = WrapText(option.Reply, 76);

        int y = Game.Height / 2 - 4;

        foreach (string line in lines)
        {
            WriteCentered(line, y, replyColor);
            y++;
        }

        if (!option.Correct)
        {
            WriteCentered(
                "That answer did not help. Try again...",
                Game.Height / 2 + 2,
                ConsoleColor.DarkYellow
            );
        }

        WriteCentered(
            "Press any key to continue...",
            Game.Height / 2 + 5,
            ConsoleColor.Gray
        );

        Console.ReadKey(true);
    }
}
