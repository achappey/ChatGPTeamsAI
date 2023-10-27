using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class SearchHit
{

    [FormColumn]
    public string? Summary { get; set; }

    [ListColumn]
    public string? ResourceName
    {
        get
        {
            return Resource?.Name;
        }
        set { }
    }

    [LinkColumn]
    public string? ResourceWebUrl
    {
        get
        {
            return Resource?.WebUrl;
        }
        set { }
    }

    [ListColumn]
    public string? LastModifiedBy
    {
        get
        {
            return Resource?.LastModifiedBy?.User?.DisplayName;
        }
        set { }
    }

    [ListColumn]
    public string? LastModifiedDateTime
    {
        get
        {
            return Resource?.LastModifiedDateTime?.ToString();
        }
        set { }
    }

    public Resource? Resource { get; set; }
}

internal class Resource
{
    public string? WebUrl { get; set; }

    public string? Name { get; set; }

    public DateTimeOffset? LastModifiedDateTime { get; set; }

    public IdentitySet? LastModifiedBy { get; set; }
}