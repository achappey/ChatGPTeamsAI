namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class Absence
{
    [JsonPropertyName("employeeName")]
    [ListColumn]
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

    [JsonPropertyName("absenceTypeLabel")]
    [FormColumn]
    public string? AbsenceTypeLabel
    {
        get
        {
            return AbsenceType?.Label;
        }
        set { }
    }

    [JsonPropertyName("absencetype")]
    public AbsenceType? AbsenceType { get; set; }

    [JsonPropertyName("employee")]
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
}

internal class AbsenceEmployee
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class AbsenceType
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

