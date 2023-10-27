

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Sales
{
    [JsonPropertyName("subject")]
    [ListColumn]
    public string? Subject { get; set; }

    [JsonPropertyName("responsibleEmployeeName")]
    [ListColumn]
    public string? ResponsibleEmployeeName
    {
        get
        {
            return ResponsibleEmployee?.Name;
        }
        set { }
    }

    [JsonPropertyName("organizationName")]
    [ListColumn]
    public string? OrganizationeName
    {
        get
        {
            return Organization?.Name;
        }
        set { }
    }

    [JsonPropertyName("statusLabel")]
    [ListColumn]
    public string? StatusLabel
    {
        get
        {
            return Status?.Label;
        }
        set { }
    }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("expected_revenue")]
    [FormColumn]
    public double? ExpectedRevenue { get; set; }

    [JsonPropertyName("chance_to_score")]
    [FormColumn]
    public double? ChanceToScore { get; set; }

    [JsonPropertyName("responsible_employee")]
    [Ignore]
    public SalesEmployee? ResponsibleEmployee { get; set; }

    [JsonPropertyName("organization")]
    [Ignore]
    public OrganizationSales? Organization { get; set; }

    [Ignore]
    [JsonPropertyName("person")]
    public PersonSales? Person { get; set; }

    [JsonPropertyName("status")]
    [Ignore]
    public Status? Status { get; set; }

    [JsonPropertyName("start_date")]
    [FormColumn]
    public string? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    [FormColumn]
    public string? EndDate { get; set; }

    [JsonPropertyName("expected_closing_date")]
    [FormColumn]
    public string? ExpectedClosingDate { get; set; }

    [JsonPropertyName("status_updated_at")]
    [FormColumn]
    public string? StatusUpdatedAt { get; set; }

    [JsonPropertyName("created_at")]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("note")]
    [FormColumn]
    public string? Note { get; set; }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? Simplicate { get; set; }

}


internal class PersonSales
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }
}

internal class OrganizationSales
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class SalesEmployee
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("person_id")]
    public string? PersonId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class RevenueGroup
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [ListColumn]
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

