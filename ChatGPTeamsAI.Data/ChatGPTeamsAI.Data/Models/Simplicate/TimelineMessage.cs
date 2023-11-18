namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class TimelineMessage
{
    [JsonPropertyName("created_at")]
    [ListColumn]
    [FormColumn]
    public string? CreatedAt { get; set; }


    [JsonPropertyName("linkedToNames")]
    [FormColumn]
    public string? LinkedToNames
    {
        get
        {
            if (LinkedTo != null && LinkedTo.Any())
            {
                return "- " + string.Join("\r- ", LinkedTo.Select(a => $"{a.Label} ({a.Type})" ));
            }

            return string.Empty;
        }
        set { }
    }

    [JsonPropertyName("content")]
    [FormColumn]
    public string? Content
    {
        get => _content?.Substring(0, Math.Min(_content?.Length ?? 0, 200));
        set
        {
            _content = value;
        }
    }

    [JsonPropertyName("messageTypeLabel")]
    [FormColumn]
    public string? MessageTypeLabel
    {
        get
        {
            return MessageType?.Label;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("message_type")]
    public MessageType? MessageType { get; set; }


    [Ignore]
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

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
