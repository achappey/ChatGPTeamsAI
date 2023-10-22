

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class Quote
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("quote_number")]
    public string? QuoteNumber { get; set; }

    [JsonPropertyName("quote_date")]
    public string? QuoteDate { get; set; }

    [JsonPropertyName("quotestatus")]
    public QuoteStatus? QuoteStatus { get; set; }

    [JsonPropertyName("quote_subject")]
    public string? QuoteSubject { get; set; }

    [JsonPropertyName("customer_reference")]
    public string? CustomerReference { get; set; }

    [JsonPropertyName("sales_id")]
    public string? SalesId { get; set; }

    [JsonPropertyName("send_type")]
    public string? SendType { get; set; }

    [JsonPropertyName("total_excl")]
    public double TotalExcl { get; set; }
}

internal class QuoteStatus
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

}
