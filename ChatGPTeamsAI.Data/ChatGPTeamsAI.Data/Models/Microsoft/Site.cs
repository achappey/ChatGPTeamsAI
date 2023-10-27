using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Site
{

    [ListColumn]    
    public string? DisplayName { get; set; }

    [ListColumn]    
    public string? Description { get; set; }

    [LinkColumn]    
    public string? WebUrl { get; set; }

    public string? Id { get; set; }

}