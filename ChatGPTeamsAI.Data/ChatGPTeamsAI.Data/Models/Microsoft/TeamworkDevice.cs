
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class TeamworkDevice
{
    public string? Id { get; set; }

    [ListColumn]
    [FormColumn]
    public string? DeviceType { get; set; }

    [Ignore]
    public HardwareDetail? HardwareDetail { get; set; }

    [FormColumn]
    public string? Notes { get; set; }

    [ListColumn]
    [FormColumn]
    public string? HealthStatus { get; set; }

    [FormColumn]
    public string? SerialNumber
    {
        get
        {
            return HardwareDetail?.SerialNumber;
        }
        set { }
    }

    [FormColumn]
    public string? Manufacturer
    {
        get
        {
            return HardwareDetail?.Manufacturer;
        }
        set { }
    }

    [ListColumn]
    [FormColumn]
    public string? Model
    {
        get
        {
            return HardwareDetail?.Model;
        }
        set { }
    }

    [FormColumn]
    public string? ActivityState { get; set; }

    [Ignore]
    public Identity? CurrentUser { get; set; }
}

internal class HardwareDetail
{
    public string? SerialNumber { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
}
