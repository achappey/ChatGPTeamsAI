
using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Device
{
    [ListColumn]
    public string? DisplayName { get; set; }

    [ListColumn]
    public string? Model { get; set; }

    [FormColumn]
    public string? Manufacturer { get; set; }

    [FormColumn]
    public string? OperatingSystem { get; set; }

    [FormColumn]
    public string? Platform { get; set; }

    [FormColumn]
    public bool? IsManaged { get; set; }

    [FormColumn]
    public string? OperatingSystemVersion { get; set; }

    [FormColumn]
    public string? DeviceOwnership { get; set; }    

    [FormColumn]
    public DateTimeOffset? ApproximateLastSignInDateTime { get; set; }

    [FormColumn]
    public DateTimeOffset? RegistrationDateTime { get; set; }


}