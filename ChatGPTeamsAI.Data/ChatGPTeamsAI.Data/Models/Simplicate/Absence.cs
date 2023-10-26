namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class Absence
{
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
    public string? Year { get; set; } 

    [JsonPropertyName("hours")]
    public double Hours { get; set; }

    [JsonPropertyName("absencetype")]
    public AbsenceType? AbsenceType { get; set; }  
    
    [JsonPropertyName("employee")]
    public Employee? Employee { get; set; }

    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; } 

    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }   

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

