using mate.Models;

namespace mate.Interface;

public interface ILevelManagerService
{
    IReadOnlyList<Level> Levels { get; }
    bool IsCompleted(User user, Level level);
    int CountCompleted(User user);
    void CompleteLevel(User user, Level level);
}