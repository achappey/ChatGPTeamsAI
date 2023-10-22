

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class Project
{
    [JsonPropertyName("project_manager")]
    public Manager? ProjectManager { get; set; }

    [JsonPropertyName("project_status")]
    public Status? ProjectStatus { get; set; }

    [JsonPropertyName("organization")]
    public OrganizationProject? OrganizationDetails { get; set; }

    [JsonPropertyName("project_number")]
    public string? ProjectNumber { get; set; }

    [JsonPropertyName("billable")]
    public bool Billable { get; set; }

    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }

     [JsonPropertyName("name")]
    public string? Name { get; set; }
    
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

