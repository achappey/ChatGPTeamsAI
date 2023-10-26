

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class Sales
{
    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("responsibleEmployeeName")]
    public string? ResponsibleEmployeeName
    {
        get
        {
            return ResponsibleEmployee?.Name;
        }
        set { }
    }

    [JsonPropertyName("organizationName")]
    public string? OrganizationeName
    {
        get
        {
            return Organization?.Name;
        }
        set { }
    }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("responsible_employee")]
    public SalesEmployee? ResponsibleEmployee { get; set; }

    [JsonPropertyName("organization")]
    public OrganizationSales? Organization { get; set; }

    [JsonPropertyName("person")]
    public PersonSales? Person { get; set; }

    [JsonPropertyName("status")]
    public Status? Status { get; set; }

    [JsonPropertyName("simplicate_url")]
    public string? SimplicateUrl { get; set; }

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

    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

