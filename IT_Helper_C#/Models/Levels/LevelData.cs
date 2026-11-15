namespace mate.Models.Levels;

public class LevelData
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Type { get; set; } = "";
    public string Character { get; set; } = "";

    public List<DialogueStep>? Dialogue { get; set; }

    public List<string>? Code { get; set; }
    public int? BuggyLine { get; set; }
    public string? Fix { get; set; }

    public List<string>? Grid { get; set; }
}

public class DialogueStep
{
    public string Npc { get; set; } = "";
    public List<DialogueOption> Options { get; set; } = new List<DialogueOption>();
}

public class DialogueOption
{
    public string Text { get; set; } = "";
    public string Reply { get; set; } = "";
    public bool Correct { get; set; }
}
