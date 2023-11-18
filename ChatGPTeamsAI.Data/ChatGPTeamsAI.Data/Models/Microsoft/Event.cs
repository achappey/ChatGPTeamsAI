using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Event
{
    public string? Id { get; set; }

    [ListColumn]
    [FormColumn]
    [TitleColumn]
    public string? Subject { get; set; }

    [Ignore]
    [ImageColumn]
    public string? Image
    {
        get
        {
            if (string.IsNullOrEmpty(Organizer?.EmailAddress?.Address) || !Organizer.EmailAddress.Address.Contains('@'))
            {
                return string.Empty;
            }

            var domain = Organizer.EmailAddress.Address.Split('@')[1];

            return $"https://logo.clearbit.com/{domain}";
        }
    }

    [Ignore]
    public ItemBody? Body { get; set; }

    [Ignore]
    public Recipient? Organizer { get; set; }

    [FormColumn]
    public bool HasAttachments { get; set; }

    [ListColumn]
    [FormColumn]
    public string? OrganizerName
    {
        get
        {
            return Organizer?.EmailAddress?.Name;
        }
        set { }
    }

    [ListColumn]
    [FormColumn]
    public string? StartDateTime
    {
        get
        {
            return Start?.DateTime;
        }
        set { }
    }

    [FormColumn]
    public string? EndDateTime
    {
        get
        {
            return End?.DateTime;
        }
        set { }
    }

    [Ignore]
    public DateTimeTimeZone? Start { get; set; }

    [Ignore]
    public DateTimeTimeZone? End { get; set; }

    [UpdatedColumn]
    [FormColumn]
    public DateTimeOffset? LastModifiedDateTime { get; set; }

    [FormColumn]
    public string? LocationName
    {
        get
        {
            return Location?.DisplayName;
        }
        set { }
    }

    [FormColumn]
    public string? Content
    {
        get
        {
            return Body?.FormattedContent;
        }
        set { }
    }

    [Ignore]
    public IEnumerable<Attendee>? Attendees { get; set; }

    [FormColumn]
    public string? AttendeeNames
    {
        get
        {
            return Attendees != null
                ? "- " + string.Join("\r- ", Attendees.Select(a => a.EmailAddress?.Name?.ToString()))
                : string.Empty;
        }
        set { }
    }

    [Ignore]
    public OnlineMeetingInfo? OnlineMeeting { get; set; }

    [Ignore]
    public Location? Location { get; set; }

    [LinkColumn]
    public string? JoinUrl
    {
        get
        {
            return OnlineMeeting?.JoinUrl;
        }
        set { }
    }

    [LinkColumn]
    public string? WebLink { get; set; }
}

internal class OnlineMeetingInfo
{
    public string? JoinUrl { get; set; }

}


internal class Attendee
{
    public EmailAddress? EmailAddress { get; set; }

}

internal class Location
{
    public string? DisplayName { get; set; }

}