
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;
using HtmlAgilityPack;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Email
{
    public string? Id { get; set; }

    [ListColumn]
    public string? FromName
    {
        get
        {
            return From?.EmailAddress?.Name;
        }
        set { }
    }

    [ListColumn]
    public string? Subject { get; set; }

    [ListColumn]
    [FormColumn]
    public DateTimeOffset? ReceivedDateTime { get; set; }

    [LinkColumn]
    public string? WebLink { get; set; }

    [FormColumn]
    public string? BodyPreview { get; set; }

    [Ignore]
    public Recipient? From { get; set; }
  
    [Ignore]
    public ItemBody? Body { get; set; }

}

internal class ItemBody
{
    public string? Content { get; set; }

    public string? ContentType { get; set; }


    public string? FormattedContent
    {
        get
        {
            switch (ContentType)
            {
                case "Html":
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(Content);

                    return doc.DocumentNode.InnerText?.Trim();
                default:
                    return Content;
            }
        }
        set { }
    }

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