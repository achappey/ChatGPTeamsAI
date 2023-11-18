using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Group
{

    [ListColumn]
    public string? DisplayName { get; set; }

    [ListColumn]
    public string? Description { get; set; }

    public string? Id { get; set; }

    [ListColumn]
    public string? Mail { get; set; }

    [LinkColumn]
    public string? WebUrl
    {
        get
        {
            return Team?.WebUrl;
        }
        set { }
    }

    [Ignore]
    public Team? Team { get; set; }
}