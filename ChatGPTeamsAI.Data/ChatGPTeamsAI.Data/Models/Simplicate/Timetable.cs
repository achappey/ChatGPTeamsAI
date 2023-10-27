namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

public class Timetable
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [ListColumn]
    [JsonPropertyName("employeeName")]
    public string? EmployeeName
    {
        get
        {
            return Employee?.Name;
        }
        set { }
    }

    [JsonPropertyName("employee")]
    public TimetableEmployee? Employee { get; set; }

    [JsonPropertyName("created_at")]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn]
    public string? UpdatedAt { get; set; }

    [FormColumn]
    [JsonPropertyName("hourly_sales_tariff")]
    public string? HourlySalesTariff { get; set; }

    [JsonPropertyName("hourly_cost_tariff")]
    [FormColumn]
    public string? HourlyCostTariff { get; set; }

    [JsonPropertyName("even_week")]
    public Week? EvenWeek { get; set; }

    [JsonPropertyName("odd_week")]
    public Week? OddWeek { get; set; }

    [JsonPropertyName("start_date")]
    [FormColumn]
    public string? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    [FormColumn]
    public string? EndDate { get; set; }

    [JsonPropertyName("productivity_target")]
    [FormColumn]
    public double ProductivityTarget { get; set; }

    [JsonPropertyName("should_write_hours")]
    [FormColumn]
    public bool ShouldWriteHours { get; set; }

    [JsonPropertyName("oddWeekSummary")]
    [FormColumn]
    public string? OddWeekSummary
    {
        get
        {
            return OddWeek != null ? GenerateWeekSummary(OddWeek) : string.Empty;
        }
        set { }
    }

    [JsonPropertyName("evenWeekSummary")]
    [FormColumn]
    public string? EvenWeekSummary
    {
        get
        {
            return EvenWeek != null ? GenerateWeekSummary(EvenWeek) : string.Empty;
        }
        set { }
    }

    private string? GenerateWeekSummary(Week week)
    {
        if (week == null) return null;

        return string.Format("Mo: {0}, Tu: {1}, We: {2}, Th: {3}, Fr: {4}, Sa: {5}, Su: {6}",
            week?.Day_1?.Hours,
            week?.Day_2?.Hours,
            week?.Day_3?.Hours,
            week?.Day_4?.Hours,
            week?.Day_5?.Hours,
            week?.Day_6?.Hours,
            week?.Day_7?.Hours);
    }
}

public class TimetableEmployee
{

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Week
{
    [JsonPropertyName("day_1")]
    public Day? Day_1 { get; set; }

    [JsonPropertyName("day_2")]
    public Day? Day_2 { get; set; }

    [JsonPropertyName("day_3")]
    public Day? Day_3 { get; set; }

    [JsonPropertyName("day_4")]
    public Day? Day_4 { get; set; }

    [JsonPropertyName("day_5")]
    public Day? Day_5 { get; set; }

    [JsonPropertyName("day_6")]
    public Day? Day_6 { get; set; }

    [JsonPropertyName("day_7")]
    public Day? Day_7 { get; set; }
}

public class Day
{
    [JsonPropertyName("start_time")]
    public double StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public double EndTime { get; set; }

    [JsonPropertyName("hours")]
    public double Hours { get; set; }
}
