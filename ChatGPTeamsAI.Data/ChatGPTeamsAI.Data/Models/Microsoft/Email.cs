
namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Email
{
    public string? Id { get; set; }
    public string? Subject { get; set; }
    public string? WebLink { get; set; }
    public ItemBody? Body { get; set; }
    public Recipient? From { get; set; }
}

internal class ItemBody
{
 //   public string Content { get; set; }

}

internal class Recipient
{
    public EmailAddress? EmailAddress { get; set; }

}
internal class EmailAddress
{
    public string? Address { get; set; }

}