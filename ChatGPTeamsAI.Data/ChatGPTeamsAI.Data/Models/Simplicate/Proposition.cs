namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class Proposition
{

    [JsonPropertyName("total_hours")]
    public double TotalHours { get; set; }

    [JsonPropertyName("total_mileage")]
    public double TotalMileage { get; set; }

    [JsonPropertyName("total_fixed")]
    public double TotalFixed { get; set; }

    [JsonPropertyName("total_terms")]
    public double TotalTerms { get; set; }

    [JsonPropertyName("total_purchase")]
    public double TotalPurchase { get; set; }

    [JsonPropertyName("total_advance_deposit")]
    public double TotalAdvanceDeposit { get; set; }

    [JsonPropertyName("total_future")]
    public double TotalFuture { get; set; }

    [JsonPropertyName("simplicate_url")]
    public string? SimplicateUrl { get; set; }

    [JsonPropertyName("project")]
    public PropositionProject? Project { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

}

internal class PropositionProject
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("project_number")]
    public string? ProjectNumber { get; set; }
}