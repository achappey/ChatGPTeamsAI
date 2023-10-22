using System;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

public class ManagedDevice
{
    public string UserDisplayName { get; set; }
    public string SerialNumber { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string OperatingSystem { get; set; }
    public DateTimeOffset EnrolledDateTime { get; set; }
    public DateTimeOffset LastSyncDateTime { get; set; }
    
    
    
}