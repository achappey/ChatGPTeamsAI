

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Quote
{
    [ListColumn]
    [JsonPropertyName("quote_number")]
    public string? QuoteNumber { get; set; }

    [ListColumn]
    [JsonPropertyName("quote_date")]
    public string? QuoteDate { get; set; }

    [ListColumn]
    [FormColumn]
    [JsonPropertyName("quote_subject")]
    public string? QuoteSubject { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("quotestatus")]
    [Ignore]
    public QuoteStatus? QuoteStatus { get; set; }

    [JsonPropertyName("customer_reference")]
    [FormColumn]
    public string? CustomerReference { get; set; }

    [JsonPropertyName("quoteStatusLabel")]
    [FormColumn]
    public string? QuoteStatusLabel
    {
        get
        {
            return QuoteStatus?.Label;
        }
        set { }
    }
    [JsonPropertyName("sales_id")]
    public string? SalesId { get; set; }

    [JsonPropertyName("send_type")]
    [FormColumn]
    public string? SendType { get; set; }

    [JsonPropertyName("total_excl")]
    [FormColumn]
    public double TotalExcl { get; set; }

}

internal class QuoteStatus
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

}
