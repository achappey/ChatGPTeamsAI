
using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class Device
{
    [ListColumn]
    public string? DisplayName { get; set; }

    [ListColumn]
    public string? Model { get; set; }

    [ListColumn]
    public string? Manufacturer { get; set; }

    [FormColumn]
    public string? OperatingSystem { get; set; }
    
    
}