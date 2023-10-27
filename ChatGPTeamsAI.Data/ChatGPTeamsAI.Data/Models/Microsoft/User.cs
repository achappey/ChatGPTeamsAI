using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class User
{
    [ListColumn]
    public string? DisplayName { get; set; }

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
    public string? SkillNames
    {
        get
        {
            return Skills != null ? string.Join(",", Skills) : null;
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