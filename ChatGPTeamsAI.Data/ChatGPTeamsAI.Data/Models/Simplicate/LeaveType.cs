

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class LeaveType
{

    [ListColumn]
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("blocked")]
    public bool Blocked { get; set; }

    [ListColumn]    
    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [ListColumn]
    [JsonPropertyName("affects_balance")]
    public bool AffectsBalance { get; set; }
}