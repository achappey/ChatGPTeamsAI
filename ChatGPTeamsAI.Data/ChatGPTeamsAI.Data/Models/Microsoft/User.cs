using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class User
{
    [ListColumn]
    public string? DisplayName { get; set; }

    [FormColumn]
    public string? EmployeeId { get; set; }

    public string? Id { get; set; }

    [ListColumn]
    public string? Department { get; set; }

    [FormColumn]
    public string? MobilePhone { get; set; }

    [FormColumn]
    public string? Mail { get; set; }

    [ListColumn]
    public string? JobTitle { get; set; }

    [FormColumn]
    public string? AboutMe { get; set; }

    [FormColumn]
    public string? PreferredLanguage { get; set; }

    [FormColumn]
    public bool AccountEnabled { get; set; }

    [FormColumn]
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

    public IEnumerable<string>? Skills { get; set; }

    [FormColumn]
    public DateTimeOffset? EmployeeHireDate { get; set; }

    [FormColumn]
    public DateTimeOffset? CreatedDateTime { get; set; }

    [FormColumn]
    public string? AlternativeMail
    {
        get
        {
            return OtherMails?.FirstOrDefault();
        }
        set { }
    }

    [FormColumn]
    public int? AssignedLicenseCount
    {
        get
        {
            return AssignedLicenses?.Count();
        }
        set { }
    }

    public IEnumerable<string>? OtherMails { get; set; }

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