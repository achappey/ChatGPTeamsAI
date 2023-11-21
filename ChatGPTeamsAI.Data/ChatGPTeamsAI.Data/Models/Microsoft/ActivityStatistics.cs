using ChatGPTeamsAI.Data.Attributes;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class ActivityStatistics
{
    [JsonProperty("@odata.type")]
    public string? OdataType { get; set; }

    [ListColumn]
    [FormColumn]
    public string? Activity { get; set; }

    [FormColumn]
    public DateTime? StartDate { get; set; }

    [ListColumn]
    [FormColumn]
    public DateTime? EndDate { get; set; }

    public string? Id { get; set; }

    [FormColumn]
    public string? TimeZoneUsed { get; set; }

    [ListColumn]
    [FormColumn]
    public Duration? Duration { get; set; }

}
