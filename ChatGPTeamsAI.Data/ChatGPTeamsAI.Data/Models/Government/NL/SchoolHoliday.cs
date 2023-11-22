
using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Government.NL;

internal class SchoolHoliday
{
    public string? Id { get; set; }

    [Ignore]
    public List<Content>? Content { get; set; }

    [FormColumn]
    public string? Title
    {
        get
        {
            return Content != null ? string.Join(", ", Content.Select(a => a.Title.Sanitize())) : string.Empty;
        }
        set { }
    }

    [ListColumn]
    [FormColumn]
    public string? SchoolYear
    {
        get
        {
            return Content != null ? string.Join(", ", Content.Select(a => a.SchoolYear.Sanitize())) : string.Empty;
        }
        set { }
    }

    [FormColumn]
    public string? Holidays
    {
        get
        {
            if (Content == null)
                return string.Empty;

            var types = Content.SelectMany(a => a.Vacations!)
                               .Where(v => v != null && v.Type != null)
                               .Select(v => v.Type!.Trim());

            return string.Join(", ", types);
        }
        set { }
    }

    private string? _notice;

    [FormColumn]
    public string? Notice
    {
        get => _notice?.Sanitize();
        set => _notice = value;
    }

    [Ignore]
    public List<string>? Authorities { get; set; }

    [Ignore]
    public List<string>? Creators { get; set; }

    [FormColumn]
    public string? License { get; set; }

    [Ignore]
    public List<string>? Rightsholders { get; set; }

    [FormColumn]
    public string? Language { get; set; }

    [FormColumn]
    public string? Location { get; set; }

    [ListColumn]
    [FormColumn]
    public DateTime? LastModified { get; set; }

    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? GetNLSchoolHolidaysBySchoolYear
    {
        get { return SchoolYear != null ? new Dictionary<string, object>() { { "schoolYear", SchoolYear } } : null; }
        set { }
    }
}

internal class Content
{
    public string? Title { get; set; }
    public string? SchoolYear { get; set; }
    public List<Vacation>? Vacations { get; set; }
}

internal class Vacation
{
    private string? _type;

    [FormColumn]
    public string? Type
    {
        get => _type?.Sanitize();
        set => _type = value;
    }

    public string? CompulsoryDates { get; set; }

    public List<RegionData>? Regions { get; set; }
}

internal class RegionData
{
    public string? Region { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}


internal class VacationRegionData
{
    [FormColumn]
    [ListColumn]
    public string? Type { get; set; }

    [ListColumn]
    [FormColumn]
    public string? Region { get; set; }

    [FormColumn]
    public DateTime StartDate { get; set; }

    [FormColumn]
    public DateTime EndDate { get; set; }
}