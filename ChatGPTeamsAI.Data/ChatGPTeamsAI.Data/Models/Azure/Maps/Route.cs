
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Azure.Maps;

internal class Route
{
    [ListColumn]
    [FormColumn("Summary")]
    public TimeSpan? TravelTimeDuration
    {
        get
        {
            return Summary?.TravelTimeDuration;
        }
        set { }
    }

    [ListColumn]
    [FormColumn("Summary")]
    public int? LengthInMeters
    {
        get
        {
            return Summary?.LengthInMeters;
        }
        set { }
    }

    [FormColumn("Summary")]
    public DateTimeOffset? ArrivalTime
    {
        get
        {
            return Summary?.ArrivalTime;
        }
        set { }
    }

    [FormColumn("Summary")]
    public DateTimeOffset? DepartureTime
    {
        get
        {
            return Summary?.DepartureTime;
        }
        set { }
    }

    [Ignore]
    public RouteSummary? Summary { get; set; }
}


internal class RouteSummary
{
    public TimeSpan? TravelTimeDuration { get; set; }
    public DateTimeOffset? ArrivalTime { get; set; }
    public DateTimeOffset? DepartureTime { get; set; }
    public int? LengthInMeters { get; set; }
}