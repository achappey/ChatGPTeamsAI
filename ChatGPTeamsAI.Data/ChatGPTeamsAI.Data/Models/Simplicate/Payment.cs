namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class Payment
{

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("invoice_id")]
    public string? InvoiceId { get; set; }

    [JsonPropertyName("date")]
    [ListColumn]
    [FormColumn]
    public string? Date { get; set; }

    [JsonPropertyName("amount")]
    [FormColumn]
    [ListColumn]
    public double? Amount { get; set; }

    [JsonPropertyName("description")]
    [FormColumn]
    [ListColumn]
    public string? Description { get; set; }
}
