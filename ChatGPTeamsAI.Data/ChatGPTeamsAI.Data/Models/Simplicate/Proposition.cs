namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Proposition
{

    [ListColumn]
    [JsonPropertyName("projectNumber")]
    public string? ProjectNumber
    {
        get
        {
            return Project?.ProjectNumber;
        }
        set { }
    }

    [ListColumn]
    [JsonPropertyName("projectName")]
    public string? ProjectName
    {
        get
        {
            return Project?.Name;
        }
        set { }
    }

    [JsonPropertyName("total_hours")]
    [ListColumn]
    public double TotalHours { get; set; }

    [JsonPropertyName("total_mileage")]
    [FormColumn]
    public double TotalMileage { get; set; }

    [JsonPropertyName("total_fixed")]
    [FormColumn]
    public double TotalFixed { get; set; }

    [JsonPropertyName("total_terms")]
    [FormColumn]
    public double TotalTerms { get; set; }

    [JsonPropertyName("total_purchase")]
    [FormColumn]
    public double TotalPurchase { get; set; }

    [JsonPropertyName("total_advance_deposit")]
    [FormColumn]
    public double TotalAdvanceDeposit { get; set; }

    [JsonPropertyName("total_future")]
    [FormColumn]
    public double TotalFuture { get; set; }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? Simplicate { get; set; }

    [JsonPropertyName("project")]
    [Ignore]
    public PropositionProject? Project { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("getProject")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetProject
    {
        get { return Project != null ? new Dictionary<string, object?>() { { "projectId", Project.Id } } : null; }
        set { }
    }
}

internal class PropositionProject
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("project_number")]
    public string? ProjectNumber { get; set; }
}