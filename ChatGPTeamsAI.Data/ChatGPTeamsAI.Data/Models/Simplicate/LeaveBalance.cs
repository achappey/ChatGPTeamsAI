namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class LeaveBalance
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

    [JsonPropertyName("balance")]
    [ListColumn]
    public double Balance { get; set; }

    [JsonPropertyName("year")]
    [ListColumn]
    public int Year { get; set; }

    [JsonPropertyName("leaveTypeLabel")]
    [FormColumn]
    public string? LeaveTypeLabel
    {
        get
        {
            return LeaveType?.Label;
        }
        set { }
    }

    [JsonPropertyName("leavetype")]
    public LeaveType? LeaveType { get; set; }

    [JsonPropertyName("employee")]
    public LeaveBalanceEmployee? Employee { get; set; }


}

internal class LeaveBalanceEmployee
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

