
namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Insights
{
    public ResourceVisualization? ResourceVisualization { get; set; }
    public ResourceReference? ResourceReference { get; set; }

}

internal class UsedInsight : Insights
{
}

internal class Trending : Insights
{
}

internal class SharedInsight : Insights
{
}

internal class ResourceVisualization
{
    public string? Title { get; set; }
    public string? Type { get; set; }

}

internal class ResourceReference
{
    public string? WebUrl { get; set; }
}

internal enum ResourceType
{
    PowerPoint,
    Word,
    Excel,
    Pdf,
    OneNote,
    OneNotePage,
    InfoPath,
    Visio,
    Publisher,
    Project,
    Access,
    Mail,
    Csv,
    Archive,
    Xps,
    Audio,
    Video,
    Image,
    Web,
    Text,
    Xml,
    Story,
    ExternalContent,
    Folder,
    Spsite,
    Other
}

