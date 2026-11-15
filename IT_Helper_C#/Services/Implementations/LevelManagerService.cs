using IT_Helper_C_.Services.Interface;
using mate.Enum;
using mate.Interface;
using mate.Models;
using mate.Models.Levels;
using System.Text.Json;

namespace mate.Services;

public class LevelManagerService : ILevelManagerService
{
    private readonly IGameService _gameService;
    private readonly IUserService _userService;
    private readonly List<Level> _levels;

    public IReadOnlyList<Level> Levels => _levels;

    public LevelManagerService(IUserService userService, IGameService gameService)
    {
        _userService = userService;
        _gameService = gameService;
        _levels = LoadLevels();
    }

    public bool IsCompleted(User user, Level level)
    {
        return user.CompleteLevelIds.Contains(level.Id);
    }

    public int CountCompleted(User user)
    {
        return _levels.Count(level => user.CompleteLevelIds.Contains(level.Id));
    }

    public void CompleteLevel(User user, Level level)
    {
        _userService.CompleteLevel(user, level.Id, _levels.Count);
    }

    private List<Level> LoadLevels()
    {
        List<Level> levels = new List<Level>();

        try
        {
            string path = "..\\..\\..\\Data\\levels.json";

            if (!File.Exists(path))
                return levels;

            string json = File.ReadAllText(path);

            List<LevelData>? data = JsonSerializer.Deserialize<List<LevelData>>(json);

            if (data == null)
                return levels;

            foreach (LevelData item in data)
            {
                Level? level = CreateLevel(item);

                if (level != null)
                    levels.Add(level);
            }
        }
        catch (Exception)
        {
        }

        return levels.OrderBy(x => x.Id).ToList();
    }

    private Level? CreateLevel(LevelData data)
    {
        if (!LevelTypes.TryParse(data.Type, ignoreCase: true, out LevelTypes type))
            return null;

        switch (type)
        {
            case LevelTypes.Dialogue:
                return new DialogueLevel(
                    _gameService,
                    data.Id,
                    data.Name,
                    data.Description,
                    data.Character == "" ? "Customer" : data.Character,
                    data.Dialogue ?? new List<DialogueStep>()
                );

            case LevelTypes.Code:
                return new CodeLevel(
                    _gameService,
                    data.Id,
                    data.Name,
                    data.Description,
                    data.Code ?? new List<string>(),
                    data.BuggyLine ?? 0,
                    data.Fix ?? ""
                );

            case LevelTypes.Movement:
                return new MovementLevel(
                    _gameService,
                    data.Id,
                    data.Name,
                    data.Description,
                    data.Grid ?? new List<string>()
                );

            default:
                return null;
        }
    }
}