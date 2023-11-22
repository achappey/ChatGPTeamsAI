using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Team
{
    [ListColumn]
    [FormColumn]
    public string? DisplayName { get; set; }

    [ListColumn]
    [FormColumn]
    public string? Description { get; set; }

    public string? Id { get; set; }

    [LinkColumn]
    public string? WebUrl { get; set; }

    [Ignore]
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

    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? GetTeamMembers
    {
        get { return Id != null ? new Dictionary<string, object>() { { "teamId", Id } } : null; }
        set { }
    }

    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? GetTeamChannels
    {
        get { return Id != null ? new Dictionary<string, object>() { { "teamId", Id } } : null; }
        set { }
    }

}

internal class TeamMember
{
    [ListColumn]
    [FormColumn]
    public string? DisplayName { get; set; }
    
    public string? Id { get; set; }

}