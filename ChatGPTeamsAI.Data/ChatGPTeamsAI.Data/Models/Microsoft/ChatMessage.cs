using AutoMapper.Configuration.Annotations;
using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class ChatMessage
{
    [Ignore]
    public ItemBody? Body { get; set; }

    [Ignore]
    public IdentitySet? From { get; set; }

    public string? Id { get; set; }

    [LinkColumn]
    public string? WebUrl { get; set; }

    [ListColumn]
    [FormColumn]
    public string? FromName
    {
        get
        {
            return From?.User?.DisplayName;
        }
        set { }
    }

    [FormColumn]
    public string? Summary { get; set; }

    [FormColumn]
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