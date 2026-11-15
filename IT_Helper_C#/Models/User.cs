using mate.Enum;
using System.ComponentModel.DataAnnotations;

namespace mate.Models;

public class User
{

    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Not a string!")]
    public string Username { get; set; }
    [
        Required(ErrorMessage = "The password is required"),
        MinLength(8, ErrorMessage = "The password is too short"),
        MaxLength(16, ErrorMessage = "The password is too long")
    ]
    public string Password { get; set; }
    public Role Role { get; set; }
    public string CreatedAt { get; set; }
    [Range(0, 100)]
    public int Progress = 0;
    public List<int> CompleteLevelIds = new List<int>();

    public User(int id, string username, string password, Role role, string createdAt)
    {
        Id = id;
        Username = username;
        Password = password;
        Role = role;
        CreatedAt = createdAt;
    }
}
