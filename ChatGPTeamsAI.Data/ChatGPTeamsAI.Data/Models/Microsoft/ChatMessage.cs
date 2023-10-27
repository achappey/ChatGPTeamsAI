using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class ChatMessage
{
    public ItemBody? Body { get; set; }

    public IdentitySet? From { get; set; }

    public string? Id { get; set; }

    public string? WebUrl { get; set; }

    [ListColumn]
    public string? FromName
    {
        get
        {
            return From?.User?.DisplayName;
        }
        set { }
    }

    [ListColumn]
    public string? Summary { get; set; }

    [ListColumn]
    public DateTimeOffset? CreatedDateTime { get; set; }
}

internal class IdentitySet
{
    public Identity? User { get; set; }

}


internal class Identity
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }


}