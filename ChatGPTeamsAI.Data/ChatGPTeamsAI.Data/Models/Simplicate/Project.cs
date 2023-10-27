

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Project
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("project_manager")]
    [Ignore]
    public Manager? ProjectManager { get; set; }

    [JsonPropertyName("project_status")]
    [Ignore]
    public Status? ProjectStatus { get; set; }

    [JsonPropertyName("organization")]
    [Ignore]
    public OrganizationProject? OrganizationDetails { get; set; }

    [JsonPropertyName("project_number")]
    [ListColumn]
    public string? ProjectNumber { get; set; }

    [JsonPropertyName("billable")]
    [FormColumn]
    public bool Billable { get; set; }

    [JsonPropertyName("start_date")]
    [FormColumn]
    public string? StartDate { get; set; }

    [FormColumn]
    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }

    [ListColumn]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [ListColumn]
    [JsonPropertyName("projectManagerName")]
    public string? ProjectManagerName
    {
        get
        {
            return ProjectManager?.Name;
        }
        set { }
    }

    [JsonPropertyName("note")]
    [FormColumn]
    public string? Note { get; set; }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? Simplicate { get; set; }
}

internal class OrganizationProject
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

internal class Manager
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class Status
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }

}

