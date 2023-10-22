

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class LeaveType
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("blocked")]
    public bool Blocked { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("affects_balance")]
    public bool AffectsBalance { get; set; }
}