
using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Email
{
    public string? Id { get; set; }

    [ListColumn]
    public string? Subject { get; set; }

    [LinkColumn]
    public string? WebLink { get; set; }

    [ListColumn]
    public string? BodyPreview { get; set; }

    [ListColumn]
    public string? FromAddress
    {
        get
        {
            return From?.EmailAddress?.Name;
        }
        set { }
    }
/*
    [FormColumn]
    public string? Content
    {
        get
        {
            return Body?.Content;
        }
        set { }
    }*/

    public Recipient? From { get; set; }

    public DateTimeOffset? ReceivedDateTime { get; set; }

   // public ItemBody? Body { get; set; }

}

internal class ItemBody
{
    public string? Content { get; set; }

}

internal class Recipient
{
    public EmailAddress? EmailAddress { get; set; }

}
internal class EmailAddress
{
    public string? Address { get; set; }
    public string? Name { get; set; }

}