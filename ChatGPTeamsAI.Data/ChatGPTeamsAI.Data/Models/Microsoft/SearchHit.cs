using System.Web;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class SearchHit
{
    public string? Summary { get; set; }
    public Resource? Resource { get; set; }
}

internal class Resource
{
    public string? WebUrl { get; set; }

    public string? Name { get; set; }
    
    public DateTimeOffset? LastModifiedDateTime { get; set; }
}