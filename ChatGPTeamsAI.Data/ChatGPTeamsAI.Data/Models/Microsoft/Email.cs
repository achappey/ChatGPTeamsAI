
using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;
using HtmlAgilityPack;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Email
{
    public string? Id { get; set; }

    [ListColumn]
    [FormColumn("Message")]
    public string? FromName
    {
        get
        {
            return From?.EmailAddress?.Name;
        }
        set { }
    }

    [Ignore]
    [ImageColumn]
    public string? Image
    {
        get
        {
            if (string.IsNullOrEmpty(FromEmail) || !FromEmail.Contains('@'))
            {
                return string.Empty;
            }

            var domain = FromEmail.Split('@')[1];

            return $"https://logo.clearbit.com/{domain}";
        }
    }

    [FormColumn("Message")]
    public string? FromEmail
    {
        get
        {
            return From?.EmailAddress?.Address;
        }
        set { }
    }

    [FormColumn("Message")]
    public string? ToNames
    {
        get
        {
            return ToRecipients != null && ToRecipients.Any() ? "- " + string.Join("\r- ", ToRecipients.Select(a => a.EmailAddress?.Name)) : string.Empty;
        }
        set { }
    }

    [FormColumn("Message")]
    public string? CcNames
    {
        get
        {
            return CcRecipients != null && CcRecipients.Any() ? "- " + string.Join("\r- ", CcRecipients.Select(a => a.EmailAddress?.Name)) : string.Empty;
        }
        set { }
    }

    [FormColumn("Message")]
    public string? BccNames
    {
        get
        {
            return BccRecipients != null && BccRecipients.Any() ? "- " + string.Join("\r- ", BccRecipients.Select(a => a.EmailAddress?.Name)) : string.Empty;
        }
        set { }
    }

    [FormColumn("Message")]
    [TitleColumn]
    [ListColumn]
    public string? Subject { get; set; }

    [FormColumn("Message")]
    public DateTimeOffset? ReceivedDateTime { get; set; }

    [FormColumn("Message")]
    public DateTimeOffset? SentDateTime { get; set; }

    [FormColumn("Message")]
    [UpdatedColumn]
    public DateTimeOffset? LastModifiedDateTime { get; set; }

    [FormColumn("Message")]
    public bool? HasAttachments { get; set; }

    [FormColumn("Options")]
    public string? Importance { get; set; }

    [FormColumn("Options")]
    public bool? IsDeliveryReceiptRequested { get; set; }

    [FormColumn("Options")]
    public bool? IsReadReceiptRequested { get; set; }

    [FormColumn("Options")]
    public bool? IsRead { get; set; }

    [FormColumn("Message")]
    public bool? IsDraft { get; set; }

    [LinkColumn]
    public string? WebLink { get; set; }

    [FormColumn("Message")]
    public string? BodyPreview { get; set; }

    [Ignore]
    public Recipient? From { get; set; }

    [Ignore]
    public IEnumerable<Recipient>? ToRecipients { get; set; }

    [Ignore]
    public IEnumerable<Recipient>? CcRecipients { get; set; }

    [Ignore]
    public IEnumerable<Recipient>? BccRecipients { get; set; }

    [Ignore]
    public ItemBody? Body { get; set; }

    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? DownloadEmail
    {
        get { return Id != null ? new Dictionary<string, object?>() { { "messageId", Id } } : null; }
        set { }
    }

    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? ReplyMail
    {
        get { return Id != null ? new Dictionary<string, object>() { { "id", Id } } : null; }
        set { }
    }
}

internal class ItemBody
{
    [JsonIgnore]
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