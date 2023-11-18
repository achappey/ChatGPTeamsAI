using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class User
{
    [ListColumn]
    [FormColumn("Identity")]
    public string? DisplayName { get; set; }

    [FormColumn("Job Information")]
    public string? EmployeeId { get; set; }

    public string? Id { get; set; }

    [ListColumn]
    [FormColumn("Job Information")]
    public string? JobTitle { get; set; }

    [ListColumn]
    [FormColumn("Job Information")]
    public string? Department { get; set; }

    [FormColumn("Job Information")]
    public string? CompanyName { get; set; }

    [FormColumn("Contact Information")]
    public string? MobilePhone { get; set; }

    [FormColumn("Contact Information")]
    public string? Mail { get; set; }

    [FormColumn("Identity")]
    public string? AboutMe { get; set; }

    [FormColumn("Identity")]
    public string? PreferredLanguage { get; set; }

    [FormColumn("Settings")]
    public bool AccountEnabled { get; set; }

    [FormColumn("Identity")]
    public string? SkillNames
    {
        get
        {
            return Skills != null ? string.Join("\r", Skills) : null;
        }
        set { }
    }

    [LinkColumn]
    public string? SendMail
    {
        get
        {
            return !string.IsNullOrEmpty(Mail) ? $"mailto:{Mail}" : null;
        }
        set { }
    }

    [LinkColumn]
    public string? Call
    {
        get
        {
            return !string.IsNullOrEmpty(MobilePhone) ? $"tel:{MobilePhone}" : null;
        }
        set { }
    }

    [Ignore]
    public IEnumerable<string>? Skills { get; set; }

    [FormColumn("Job Information")]
    public DateTimeOffset? EmployeeHireDate { get; set; }

    [FormColumn("Identity")]
    public DateTimeOffset? CreatedDateTime { get; set; }

    [FormColumn("Contact Information")]
    public string? AlternativeMail
    {
        get
        {
            return OtherMails?.FirstOrDefault();
        }
        set { }
    }

    [FormColumn("Identity")]
    public int? AssignedLicenseCount
    {
        get
        {
            return AssignedLicenses?.Count();
        }
        set { }
    }

    [Ignore]
    public IEnumerable<string>? OtherMails { get; set; }

    [Ignore]
    public IEnumerable<AssignedLicense>? AssignedLicenses { get; set; }

    [LinkColumn]
    public string? Profile
    {
        get
        {
            return $"https://eur.delve.office.com/?u={Id}";
        }
        set { }
    }
}


internal class AssignedLicense
{
}