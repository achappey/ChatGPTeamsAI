namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class LeaveBalance
{
    [JsonPropertyName("employeeName")]
    [FormColumn]
    public string? EmployeeName
    {
        get
        {
            return Employee?.Name;
        }
        set { }
    }

    [JsonPropertyName("leaveTypeLabel")]
    [ListColumn]
    [FormColumn]
    public string? LeaveTypeLabel
    {
        get
        {
            return LeaveType?.Label;
        }
        set { }
    }

    [JsonPropertyName("balance")]
    [ListColumn]
    [FormColumn]
    public double Balance { get; set; }

    [JsonPropertyName("year")]
    [ListColumn]
    [FormColumn]
    public int Year { get; set; }

    [Ignore]
    [JsonPropertyName("leavetype")]
    public LeaveType? LeaveType { get; set; }

    [JsonPropertyName("employee")]
    [Ignore]
    public LeaveBalanceEmployee? Employee { get; set; }

    [JsonPropertyName("getEmployee")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetEmployee
    {
        get { return Employee != null ? new Dictionary<string, object?>() { { "employeeId", Employee.Id } } : null; }
        set { }
    }


}

internal class LeaveBalanceEmployee
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

