
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

public class SecureScore
{
    public string? Id { get; set; }

    [FormColumn]
    public int ActiveUserCount { get; set; }

    [FormColumn]
    [ListColumn]
    public DateTimeOffset? CreatedDateTime { get; set; }

    [FormColumn]
    [ListColumn]
    public double CurrentScore { get; set; }

    [FormColumn]
    public string? EnabledServiceNames
    {
        get
        {
            if (EnabledServices != null && EnabledServices.Any())
            {
                return "- " + string.Join("\r- ", EnabledServices);
            }
            return string.Empty;
        }
        set { }
    }

    [FormColumn]
    public string? ControlScoreNames
    {
        get
        {
            if (ControlScores != null && ControlScores.Any())
            {
                return "- " + string.Join("\r- ", ControlScores.Select(t => $"{t.Description}"));
            }
            return string.Empty;
        }
        set { }
    }

    [Ignore]
    public List<string>? EnabledServices { get; set; }

    [FormColumn]
    public int LicensedUserCount { get; set; }

    [FormColumn]
    public double MaxScore { get; set; }

    [Ignore]
    public List<AverageComparativeScore>? AverageComparativeScores { get; set; }

    [Ignore]
    public List<ControlScore>? ControlScores { get; set; }

}

public class AverageComparativeScore
{
    public string? Basis { get; set; }
    public double AverageScore { get; set; }
}

public class ControlScore
{
    public string? ControlCategory { get; set; }
    public string? ControlName { get; set; }
    public string? Description { get; set; }
    public double Score { get; set; }
}

