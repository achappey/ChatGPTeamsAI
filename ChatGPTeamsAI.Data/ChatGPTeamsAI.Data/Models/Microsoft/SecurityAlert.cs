using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class SecurityAlert
{
    public string? Id { get; set; }

    [FormColumn]
    public string? Status { get; set; }

    [FormColumn]
    [ListColumn]
    public string? Title { get; set; }

    [FormColumn]
    public string? Description { get; set; }

    [FormColumn]
    public string? RecommendedActions { get; set; }

    [LinkColumn]
    public string? AlertWebUrl { get; set; }

    [LinkColumn]
    public string? IncidentWebUrl { get; set; }

    [FormColumn]
    public DateTimeOffset? CreatedDateTime { get; set; }

    [FormColumn]
    public DateTimeOffset? LastUpdateDateTime { get; set; }

    [FormColumn]
    public DateTimeOffset? ResolvedDateTime { get; set; }

    [FormColumn]
    [ListColumn]
    public string? Category { get; set; }

    [FormColumn]
    public string? EvidenceCount
    {
        get
        {
            if (EvidenceCount != null && EvidenceCount.Any())
            {
                return EvidenceCount.Count().ToString();
            }

            return string.Empty;
        }
        set { }
    }

    [Ignore]
    public List<AlertEvidence>? Evidence { get; set; }

}

internal class AlertEvidence
{

}

