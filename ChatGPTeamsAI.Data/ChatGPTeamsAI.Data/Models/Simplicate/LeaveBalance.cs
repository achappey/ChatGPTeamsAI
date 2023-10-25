namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class LeaveBalance
{
    [JsonPropertyName("employee")]
    public LeaveBalanceEmployee? Employee { get; set; }

    [JsonPropertyName("balance")]
    public double Balance { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("leavetype")]
    public LeaveType? LeaveType { get; set; }
}

internal class LeaveBalanceEmployee
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

