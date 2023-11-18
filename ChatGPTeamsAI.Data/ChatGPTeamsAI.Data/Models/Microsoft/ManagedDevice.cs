using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

public class ManagedDevice
{
    [ListColumn]
    [FormColumn]
    public string? UserDisplayName { get; set; }

    [ListColumn]
    [FormColumn]
    public string? SerialNumber { get; set; }

    [ListColumn]
    [FormColumn]
    public string? Model { get; set; }

    [FormColumn]
    public string? Manufacturer { get; set; }

    [FormColumn]
    public string? PhoneNumber { get; set; }

    [FormColumn]
    public string? Imei { get; set; }

    [FormColumn]
    public string? OperatingSystem { get; set; }

    [FormColumn]
    public string? OsVersion { get; set; }

    [FormColumn]
    public long? PhysicalMemoryInBytes { get; set; }

    [FormColumn]
    public long? TotalStorageSpaceInBytes { get; set; }

    [FormColumn]
    public long? FreeStorageSpaceInBytes { get; set; }

    [FormColumn]
    public string? Notes { get; set; }

    [FormColumn]
    public string? ManagedDeviceOwnerType { get; set; }

    [FormColumn]
    public bool? IsEncrypted { get; set; }

    [FormColumn]
    public DateTimeOffset EnrolledDateTime { get; set; }

    [FormColumn]
    public DateTimeOffset LastSyncDateTime { get; set; }

    [FormColumn]
    public DateTimeOffset? ManagementCertificateExpirationDate { get; set; }

    [FormColumn]
    public string? UserId { get; set; }

    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetUser
    {
        get { return UserId != null ? new Dictionary<string, object?>() { { "userId", UserId } } : null; }
        set { }
    }

}