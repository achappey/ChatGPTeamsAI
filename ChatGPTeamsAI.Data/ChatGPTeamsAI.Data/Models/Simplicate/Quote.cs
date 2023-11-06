

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Quote
{
    [FormColumn]
    [JsonPropertyName("quote_number")]
    public string? QuoteNumber { get; set; }

    [ListColumn]
    [FormColumn]
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

    [JsonPropertyName("quotetemplate")]
    [Ignore]
    public QuoteTemplate? QuoteTemplate { get; set; }

    [JsonPropertyName("created_by")]
    [Ignore]
    public CreatedBy? CreatedBy { get; set; }

    [JsonPropertyName("customer_reference")]
    [FormColumn]
    public string? CustomerReference { get; set; }

    [JsonPropertyName("createdByName")]
    [FormColumn]
    public string? CreatedByName
    {
        get
        {
            return CreatedBy?.Name;
        }
        set { }
    }

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

    [JsonPropertyName("quoteTemplateName")]
    [FormColumn]
    public string? QuoteTemplateName
    {
        get
        {
            return QuoteTemplate?.Name;
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

    [JsonPropertyName("getSales")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetSales
    {
        get { return SalesId != null ? new Dictionary<string, object?>() { { "saleId", SalesId } } : null; }
        set { }
    }
}

internal class QuoteStatus
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

}


internal class QuoteTemplate
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

