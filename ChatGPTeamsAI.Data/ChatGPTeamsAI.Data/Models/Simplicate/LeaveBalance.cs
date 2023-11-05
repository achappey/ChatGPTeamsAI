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


}

internal class LeaveBalanceEmployee
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

