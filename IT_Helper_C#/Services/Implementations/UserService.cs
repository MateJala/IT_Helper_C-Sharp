using IT_Helper_C_.Services.Interface;
using mate.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace IT_Helper_C_.Services.Services;

public class UserService : IUserService
{
    private readonly string path =
        "..\\..\\..\\Data\\users.json";

    private static readonly JsonSerializerOptions JsonOptions =
        new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        };

    public bool CreateUser(User user)
    {
        var users = GetAllUsers();

        if (users.Any(x =>
            x.Username.Equals(user.Username,
                StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        if (!ValidateUser(user))
            return false;

        int maxId = users.Count > 0
            ? users.Max(x => x.Id)
            : 0;

        user.Id = maxId + 1;

        users.Add(user);

        SaveUsers(users);

        return true;
    }

    public List<User> GetAllUsers()
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(path)!
            );

            File.WriteAllText(path, "[]");

            return new List<User>();
        }

        string json = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(json))
            return new List<User>();

        return JsonSerializer.Deserialize<List<User>>(json, JsonOptions)
               ?? new List<User>();
    }

    private void SaveUsers(List<User> users)
    {
        string json = JsonSerializer.Serialize(users, JsonOptions);

        File.WriteAllText(path, json);
    }

    public User? Login(string username, string password)
    {
        var users = GetAllUsers();

        return users.FirstOrDefault(x =>
            x.Username.Equals(username,
                StringComparison.OrdinalIgnoreCase) &&
            x.Password == password
        );
    }

    public bool ValidateUser(User user)
    {
        var context = new ValidationContext(user);

        var results = new List<ValidationResult>();

        return Validator.TryValidateObject(
            user,
            context,
            results,
            validateAllProperties: true
        );
    }

    public bool CompleteLevel(User user, int levelId, int totalLevels)
    {
        if (!user.CompleteLevelIds.Contains(levelId))
            user.CompleteLevelIds.Add(levelId);

        // Progress is the percentage of completed levels
        user.Progress = totalLevels > 0
            ? (int)Math.Round(user.CompleteLevelIds.Count * 100.0 / totalLevels)
            : 0;

        var users = GetAllUsers();

        int index = users.FindIndex(x => x.Id == user.Id);

        if (index < 0)
            users.Add(user);
        else
            users[index] = user;

        SaveUsers(users);

        return true;
    }

    public bool DeleteUser(string username)
    {
        var users = GetAllUsers();

        User? user = users.FirstOrDefault(x =>
            x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
        );

        if (user == null)
            return false;

        users.Remove(user);
        SaveUsers(users);

        return true;
    }
}
