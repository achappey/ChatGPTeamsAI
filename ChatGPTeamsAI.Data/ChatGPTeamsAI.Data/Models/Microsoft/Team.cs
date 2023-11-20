using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Team
{
    [ListColumn]    
    public string? DisplayName { get; set; }

    [ListColumn]    
    public string? Description { get; set; }

    public string? Id { get; set; }

    [LinkColumn]    
    public string? WebUrl { get; set; }

    public IEnumerable<Channel>? Channels { get; set; }

    [FormColumn]
    public string? ChannelNames
    {
        get
        {
            return Channels != null ? string.Join(", ", Channels.Select(a => a.DisplayName)) : string.Empty;
        }
        set { }
    }

}

internal class TeamMember
{
    public string? DisplayName { get; set; }
    public string? Id { get; set; }

}