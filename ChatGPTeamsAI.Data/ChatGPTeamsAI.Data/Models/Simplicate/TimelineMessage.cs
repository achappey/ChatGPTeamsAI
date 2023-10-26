

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

internal class TimelineMessage
{
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("linkedToName")]
    public string? LinkedToName
    {
        get
        {
            return LinkedTo.FirstOrDefault()?.Label;
        }
        set { }
    }

    [JsonPropertyName("content")]
    public string? Content
    {
        get => _content?.Substring(0, Math.Min(_content?.Length ?? 0, 200));
        set
        {
            _content = value;
        }
    }

    private string? _content { get; set; }

    [JsonPropertyName("message_type")]
    public MessageType? MessageType { get; set; }


    [JsonPropertyName("linked_to")]
    public List<LinkedTo>? LinkedTo { get; set; }




}


internal class MessageType
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

internal class LinkedTo
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}
