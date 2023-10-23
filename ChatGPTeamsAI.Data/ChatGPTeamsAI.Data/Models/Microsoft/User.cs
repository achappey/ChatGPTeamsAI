namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class User
{
    public string? DisplayName { get; set; }
    public string? EmployeeId { get; set; }
    public string? Department { get; set; }
    public string? MobilePhone { get; set; }
    public string? Mail { get; set; }
    public string? JobTitle { get; set; }
    public string? AboutMe { get; set; }
    public IEnumerable<string>? Skills { get; set; }
    public DateTimeOffset? EmployeeHireDate { get; set; }
}