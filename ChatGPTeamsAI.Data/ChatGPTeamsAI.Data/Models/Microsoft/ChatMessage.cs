namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class ChatMessage
{
    public ItemBody? Body { get; set; }
    public ChatMessageFromIdentitySet? From { get; set; }
    public string? Id { get; set; }
    public string? WebUrl { get; set; }
    public string? Summary { get; set; }
    public DateTimeOffset? CreatedDateTime { get; set; }
}

internal class ChatMessageFromIdentitySet
{
    public Identity? User { get; set; }

}


internal class Identity
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }


}