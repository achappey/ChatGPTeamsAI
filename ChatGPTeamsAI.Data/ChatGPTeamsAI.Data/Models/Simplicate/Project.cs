

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class Project
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("project_manager")]
    public Manager? ProjectManager { get; set; }

    [JsonPropertyName("project_status")]
    public Status? ProjectStatus { get; set; }

    [JsonPropertyName("organization")]
    public OrganizationProject? OrganizationDetails { get; set; }

    [JsonPropertyName("project_number")]
    [ColumnName]
    public string? ProjectNumber { get; set; }

    [JsonPropertyName("billable")]
    public bool Billable { get; set; }

    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }

    [ColumnName]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [ColumnName]
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
    public string? Note { get; set; }


    [JsonPropertyName("simplicate_url")]
    public string? SimplicateUrl { get; set; }
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

