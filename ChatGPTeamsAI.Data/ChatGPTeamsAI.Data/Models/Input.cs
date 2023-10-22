namespace ChatGPTeamsAI.Data.Models.Input;

public class Action
{
    public required string Name { get; set; }
    public IDictionary<string, object?>? Entities { get; set; }
}
