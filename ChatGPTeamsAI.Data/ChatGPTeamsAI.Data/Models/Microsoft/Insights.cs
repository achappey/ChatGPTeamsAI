
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Insights
{
    [Ignore]
    public ResourceVisualization? ResourceVisualization { get; set; }
    
    [Ignore]
    public ResourceReference? ResourceReference { get; set; }

    [ListColumn]
    [FormColumn]
    public string? Title
    {
        get
        {
            return ResourceVisualization?.Title;
        }
        set { }
    }

    [ListColumn]
    public string? Type
    {
        get
        {
            return ResourceVisualization?.Type;
        }
        set { }
    }

    [FormColumn]
    public string? PreviewText
    {
        get
        {
            return ResourceVisualization?.PreviewText;
        }
        set { }
    }

    [LinkColumn(true)]
    public string? Url
    {
        get
        {
            return ResourceReference?.WebUrl;
        }
        set { }
    }

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
    
    public string? PreviewText { get; set; }
    
    public string? PreviewImageUrl { get; set; }

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

