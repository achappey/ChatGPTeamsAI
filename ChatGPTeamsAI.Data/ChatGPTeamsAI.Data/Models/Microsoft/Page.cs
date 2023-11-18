using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Page
{
    [FormColumn]
    [ListColumn]
    public string? Name { get; set; }

    [FormColumn]
    [ListColumn]
    public string? Description { get; set; }

    [LinkColumn]
    public string? WebUrl { get; set; }

    public string? Id { get; set; }

}