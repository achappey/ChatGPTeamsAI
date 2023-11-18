
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
    public int LicensedUserCount { get; set; }

    [FormColumn]
    public double MaxScore { get; set; }

    [Ignore]
    public List<AverageComparativeScore>? AverageComparativeScores { get; set; }


}

public class AverageComparativeScore
{
    public string? Basis { get; set; }
    public double AverageScore { get; set; }
}
