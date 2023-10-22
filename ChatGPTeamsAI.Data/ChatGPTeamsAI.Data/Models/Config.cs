namespace ChatGPTeamsAI.Data.Models;

public class Configuration
{
    public string? GraphApiToken { get; set; }
    public SimplicateToken? SimplicateToken { get; set; }
}

public class SimplicateToken
{
    public required string Environment { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
