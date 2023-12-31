namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Hour
{
    [ListColumn]
    [JsonPropertyName("start_date")]
    public string? StartDate { get; set; }

    [ListColumn]
    [JsonPropertyName("hours")]
    public double Hours { get; set; }

    [ListColumn]
    [JsonPropertyName("projectName")]
    public string? ProjectName
    {
        get
        {
            return Project?.Name;
        }
        set { }
    }

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

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [Ignore]
    [JsonPropertyName("employee")]
    public HourEmployee? Employee { get; set; }

    [Ignore]
    [JsonPropertyName("project")]
    public HourProject? Project { get; set; }

    [JsonPropertyName("projectservice")]
    [Ignore]
    public HourProjectService? ProjectService { get; set; }

    [JsonPropertyName("status")]
    [FormColumn]
    public string? Status { get; set; }

    [JsonPropertyName("tariff")]
    [FormColumn]
    public double Tariff { get; set; }

    [FormColumn]
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

    [JsonPropertyName("project_manager")]
    public HourEmployee? ProjectManager { get; set; }


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

    [ListColumn]
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("tariff")]
    public string? Tariff { get; set; }

    [ListColumn]
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


internal class HourApproval
{
    [ListColumn]
    [FormColumn]
    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [ListColumn]
    [FormColumn]
    [JsonPropertyName("approvalStatusDescription")]
    public string? ApprovalStatusDescription
    {
        get
        {
            return ApprovalStatus?.Description;
        }
        set { }
    }

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

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [Ignore]
    [JsonPropertyName("employee")]
    public HourEmployee? Employee { get; set; }

    [Ignore]
    [JsonPropertyName("approvalstatus")]
    public HourApprovalStatis? ApprovalStatus { get; set; }

}


internal class HourApprovalStatis
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

}