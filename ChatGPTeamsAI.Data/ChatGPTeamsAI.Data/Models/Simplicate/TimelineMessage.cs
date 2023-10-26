

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class TimelineMessage
{
    [JsonPropertyName("created_at")]
    [ListColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("linkedToName")]
    [ListColumn]
    public string? LinkedToName
    {
        get
        {
            return LinkedTo?.FirstOrDefault()?.Label;
        }
        set { }
    }

    [JsonPropertyName("content")]
    [ListColumn]
    public string? Content
    {
        get => _content?.Substring(0, Math.Min(_content?.Length ?? 0, 200));
        set
        {
            _content = value;
        }
    }

    [JsonPropertyName("message_type")]
    public MessageType? MessageType { get; set; }


    [JsonPropertyName("linked_to")]
    public List<LinkedTo>? LinkedTo { get; set; }

    private string? _content { get; set; }


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
