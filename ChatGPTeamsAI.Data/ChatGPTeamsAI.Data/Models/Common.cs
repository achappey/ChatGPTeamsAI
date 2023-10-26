using AdaptiveCards;
using ChatGPTeamsAI.Data.Models.Output;

namespace ChatGPTeamsAI.Data.Models;

internal interface IBaseClient
{
    internal abstract Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Input.Action action);
    internal abstract IEnumerable<ActionDescription> GetAvailableActions();
    
}

internal class ChatGPTeamsAIClientResponse
{
    public string? Data { get; set; }
    public string? Error { get; set; }
    public Input.Action? ExecutedAction { get; set; }
    public Input.Action? NextPageAction { get; set; }
    public Input.Action? PreviousPageAction { get; set; }
    public Input.Action? ExportPageAction { get; set; }
    public AdaptiveCard? DataCard { get; set; }
    public int? TotalItems { get; set; }
    public int? TotalPages { get; set; }
    public int? CurrentPage { get; set; }
    public int? ItemsPerPage { get; set; }
    public IDictionary<string, object>? Properties { get; set; }
    public required string Type { get; set; }

}

internal class NoOutputResponse
{
    public required string Status { get; set; }
    public required string Message { get; set; }
    public DateTime Timestamp { get; set; }
}