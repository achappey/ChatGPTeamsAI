namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class Summary
{
    [JsonPropertyName("description")]
    [ListColumn]
    public string? Description { get; set; }

    [JsonPropertyName("total")]
    [ListColumn]
    public double? Total { get; set; }

}
