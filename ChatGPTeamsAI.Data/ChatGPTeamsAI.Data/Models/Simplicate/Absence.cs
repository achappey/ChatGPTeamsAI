namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Absence
{
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

    [JsonPropertyName("absenceTypeLabel")]
    [ListColumn]
    public string? AbsenceTypeLabel
    {
        get
        {
            return AbsenceType?.Label;
        }
        set { }
    }

    [JsonPropertyName("year")]
    [ListColumn]
    public string? Year { get; set; }

    [JsonPropertyName("hours")]
    [ListColumn]
    public double Hours { get; set; }

    [JsonPropertyName("absencetype")]
    [Ignore]
    public AbsenceType? AbsenceType { get; set; }

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

