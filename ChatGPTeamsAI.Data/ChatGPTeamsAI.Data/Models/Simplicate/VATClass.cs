namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class VATClass
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [ListColumn]
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [ListColumn]
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [ListColumn]
    [JsonPropertyName("percentage")]
    public float Percentage { get; set; }
}
