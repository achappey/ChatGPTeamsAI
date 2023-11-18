
using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Channel
{
    [FormColumn]
    [ListColumn]
    public required string DisplayName { get; set; }

    [FormColumn]
    [ListColumn]
    public string? Description { get; set; }

    public string? Id { get; set; }

    [LinkColumn]
    public string? WebUrl { get; set; }

}