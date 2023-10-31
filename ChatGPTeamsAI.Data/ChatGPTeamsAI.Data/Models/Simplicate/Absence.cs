namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Absence
{
    [ListColumn]
    [FormColumn]
    [JsonPropertyName("employeeName")]
    public string? EmployeeName
    {
        get
        {
            return Employee?.Name;
        }
        set { }
    }

    [JsonPropertyName("year")]
    [ListColumn]
    public string? Year { get; set; }

    [JsonPropertyName("hours")]
    [ListColumn]
    public double Hours { get; set; }

    [JsonPropertyName("employee")]
    [Ignore]
    public Employee? Employee { get; set; }

    [FormColumn]
    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; }

    [FormColumn]
    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }

    [FormColumn]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("created_at")]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn]
    public string? UpdatedAt { get; set; }
}

internal class AbsenceEmployee
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
