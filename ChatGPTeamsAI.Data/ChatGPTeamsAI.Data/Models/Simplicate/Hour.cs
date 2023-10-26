namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

internal class Hour
{

    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; }

    [JsonPropertyName("hours")]
    public double Hours { get; set; }

    [JsonPropertyName("projectName")]
    public string? ProjectName
    {
        get
        {
            return Project?.Name;
        }
        set { }
    }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("employee")]
    public HourEmployee? Employee { get; set; }

    [JsonPropertyName("project")]
    public HourProject? Project { get; set; }

    [JsonPropertyName("projectservice")]
    public HourProjectService? ProjectService { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("tariff")]
    public double Tariff { get; set; }


    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }

}

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum HourStatus
{
    [EnumMember(Value = "supervisor_rejected")]
    SupervisorRejected,

    [EnumMember(Value = "supervisor_approved")]
    SupervisorApproved,

    [EnumMember(Value = "projectmanager_approved")]
    ProjectManagerApproved,

    [EnumMember(Value = "forwarded")]
    Forwarded,

    [EnumMember(Value = "to_forward")]
    ToForward
}


internal class HourProject
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}


internal class HourProjectService
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class HoursType
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("tariff")]
    public string? Tariff { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

}


internal class HourEmployee
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("person_id")]
    public string? PersonId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}