using mate.Interface;

namespace mate.Models.Levels;

public class MovementLevel : Level
{
    private readonly char[][] _grid;
    private readonly int _playerStartX;
    private readonly int _playerStartY;

    private int _playerX;
    private int _playerY;
    private int _brokenTotal;
    private int _fixedCount;

    public MovementLevel(
        IGameService game,
        int id,
        string name,
        string description,
        List<string> grid
    )
        : base(game, id, name, description)
    {
        _grid = grid.Select(x => x.ToCharArray()).ToArray();

        _brokenTotal = 0;

        for (int y = 0; y < _grid.Length; y++)
        {
            for (int x = 0; x < _grid[y].Length; x++)
            {
                if (_grid[y][x] == '#')
                    _brokenTotal++;
                else if (_grid[y][x] == 'P')
                {
                    _playerStartX = x;
                    _playerStartY = y;
                    _grid[y][x] = '.';
                }
            }
        }

        _playerX = _playerStartX;
        _playerY = _playerStartY;
        _fixedCount = 0;
    }

    public override bool Play()
    {
        _playerX = _playerStartX;
        _playerY = _playerStartY;
        _fixedCount = 0;

        DrawScreen();

        while (true)
        {
            DrawGrid();
            DrawStatus();

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    TryMove(0, -1);
                    break;

                case ConsoleKey.DownArrow:
                    TryMove(0, 1);
                    break;

                case ConsoleKey.LeftArrow:
                    TryMove(-1, 0);
                    break;

                case ConsoleKey.RightArrow:
                    TryMove(1, 0);
                    break;

                case ConsoleKey.F:
                case ConsoleKey.Spacebar:
                    if (_grid[_playerY][_playerX] == '#')
                    {
                        _grid[_playerY][_playerX] = '+';
                        _fixedCount++;

                        if (_fixedCount >= _brokenTotal)
                        {
                            ShowSuccess();
                            return true;
                        }
                    }
                    break;

                case ConsoleKey.Escape:
                    return false;
            }
        }
    }

    private void TryMove(int dx, int dy)
    {
        int newX = _playerX + dx;
        int newY = _playerY + dy;

        if (newY < 0 || newY >= _grid.Length)
            return;

        if (newX < 0 || newX >= _grid[newY].Length)
            return;

        if (_grid[newY][newX] == 'X')
            return;

        _playerX = newX;
        _playerY = newY;
    }

    private void DrawScreen()
    {
        Game.Background();

        WriteCentered($"------{Name}------", Game.Height / 2 - 12, ConsoleColor.Yellow);

        WriteCentered(
            "[Arrows] Move   [F] Fix The Node You Stand On   [Esc] Leave",
            Game.Height - 7,
            ConsoleColor.Gray
        );
    }

    private void DrawStatus()
    {
        WriteCentered(
            $"Repaired: {_fixedCount} / {_brokenTotal}",
            Game.Height / 2 - 10,
            ConsoleColor.Green
        );
    }

    private void DrawGrid()
    {
        int cellWidth = 2;

        int x0 = (Game.Width - (_grid[0].Length * cellWidth - 1)) / 2;
        int y0 = (Game.Height - _grid.Length) / 2 + 2;

        for (int y = 0; y < _grid.Length; y++)
        {
            for (int x = 0; x < _grid[y].Length; x++)
            {
                char c = _grid[y][x];

                ConsoleColor color;

                if (x == _playerX && y == _playerY)
                {
                    c = '@';
                    color = ConsoleColor.Cyan;
                }
                else if (c == '#')
                    color = ConsoleColor.Red;
                else if (c == '+')
                    color = ConsoleColor.Green;
                else
                    color = ConsoleColor.DarkGray;

                Console.ForegroundColor = color;

                Console.SetCursorPosition(x0 + x * cellWidth, y0 + y);
                Console.Write(c);

                Console.SetCursorPosition(x0 + x * cellWidth + 1, y0 + y);
                Console.Write(' ');

                Console.ResetColor();
            }
        }
    }

    private void ShowSuccess()
    {
        Game.Background();

        WriteCentered("------All Nodes Repaired!------", Game.Height / 2 - 2, ConsoleColor.Green);

        WriteCentered(
            $"Every broken node in {Name} is fixed!",
            Game.Height / 2,
            ConsoleColor.Yellow
        );

        WriteCentered(
            "Press any key to continue...",
            Game.Height / 2 + 4,
            ConsoleColor.Gray
        );

        Console.ReadKey(true);
    }
}
