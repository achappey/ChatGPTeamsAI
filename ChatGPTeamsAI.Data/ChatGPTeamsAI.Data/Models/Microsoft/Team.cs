
using System.Collections.Generic;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

public class Team
{
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Id { get; set; }
    public string WebUrl { get; set; }

    public IEnumerable<Channel> Channels { get; set; }

}
public class TeamMember
{
    public string DisplayName { get; set; }
    public string Id { get; set; }

}