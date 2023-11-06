namespace ChatGPTeamsAI.Data.Models.Output;

public class ActionResponse
{
    public string? Data { get; set; }
    public string? Error { get; set; }
    public string? DataCard { get; set; }
    public Input.Action? ExecutedAction { get; set; }
}

public class ActionDescription
{
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required string Description { get; set; }
    public required string Publisher { get; set; }
    public string? ExportAction { get; set; }
    public Parameters? Parameters { get; set; }

    public string CategoryIdentifier
    {
        get
        {
            return $"{Publisher} {Category}";
        }
        set { }

    }
}

public class Parameters
{
    public IEnumerable<Property>? Properties { get; set; }
    public IEnumerable<string>? Required { get; set; }
}

public class Property
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Description { get; set; }
    public bool? IsMultiline { get; set; }
    public bool? IsHidden { get; set; }
    public IEnumerable<string>? Enum { get; set; }
}
