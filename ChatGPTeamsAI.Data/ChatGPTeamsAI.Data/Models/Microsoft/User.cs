using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class User
{

    [Ignore]
    [ImageColumn]
    public string? Logo
    {
        get
        {
            if (string.IsNullOrEmpty(Mail) || !Mail.Contains('@'))
            {
                return string.Empty;
            }

            var domain = Mail.Split('@')[1];

            return $"https://logo.clearbit.com/{domain}";
        }
    }

    [ListColumn]
    [TitleColumn]
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
    public string? StreetAddress { get; set; }
    
    [FormColumn("Contact Information")]
    public string? PostalCode { get; set; }    

    [FormColumn("Contact Information")]
    public string? City { get; set; }

    [FormColumn("Contact Information")]
    public string? State { get; set; }

    [FormColumn("Contact Information")]
    public string? Country { get; set; }

    [FormColumn("Settings")]
    public string? UsageLocation { get; set; }

    [FormColumn("Contact Information")]
    public string? MobilePhone { get; set; }

    [FormColumn("Contact Information")]
    public string? Mail { get; set; }

    [LinkColumn]
    [Ignore]
    public string? MySite { get; set; }

    [FormColumn("Identity")]
    public string? AboutMe { get; set; }

    [FormColumn("Settings")]
    public string? ExternalUserState { get; set; }    

    [FormColumn("Identity")]
    public string? PreferredLanguage { get; set; }

    [FormColumn("Settings")]
    public bool AccountEnabled { get; set; }

    [FormColumn("Identity")]
    public string? SkillNames
    {
        get
        {
            return Skills != null ? string.Join(", ", Skills) : null;
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

    [FormColumn("Identity")]
    public DateTimeOffset? LastPasswordChangeDateTime { get; set; }

    [FormColumn("Contact Information")]
    public string? AlternativeMail
    {
        get
        {
            return OtherMails?.FirstOrDefault();
        }
        set { }
    }

    [FormColumn("Contact Information")]
    public string? BusinessPhone
    {
        get
        {
            return BusinessPhones?.FirstOrDefault();
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

    [Ignore]
    public IEnumerable<string>? BusinessPhones { get; set; }

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