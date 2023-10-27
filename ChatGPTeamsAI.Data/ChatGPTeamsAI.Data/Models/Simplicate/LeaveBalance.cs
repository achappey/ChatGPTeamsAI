namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

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
    public double Balance { get; set; }

    [JsonPropertyName("year")]
    [ListColumn]
    public int Year { get; set; }

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

