namespace ChatGPTeamsAI.Data.Models;

public class Configuration
{
    public string? GraphApiToken { get; set; }
    public string? AzureMapsSubscriptionKey { get; set; }
    public string? Locale { get; set; }
    public SimplicateToken? SimplicateToken { get; set; }
}

public class SimplicateToken
{
    public required string Environment { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
