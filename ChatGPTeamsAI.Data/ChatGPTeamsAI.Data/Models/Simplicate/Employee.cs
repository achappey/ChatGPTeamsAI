

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class Employee
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("person_id")]
    public string? PersonId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("function")]
    public string? Function { get; set; }

    [JsonPropertyName("employment_status")]
    public string? EmploymentStatus { get; set; }

    [JsonPropertyName("civil_status")]
    public string? CivilStatus { get; set; }

    [JsonPropertyName("work_phone")]
    public string? WorkPhone { get; set; }

    [JsonPropertyName("work_mobile")]
    public string? WorkMobile { get; set; }

    [JsonPropertyName("work_email")]
    public string? WorkEmail { get; set; }

   // [JsonPropertyName("hourly_sales_tariff")]
   // public double? HourlySalesTariff { get; set; }

   // [JsonPropertyName("hourly_cost_tariff")]
//    public double? HourlyCostTariff { get; set; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("simplicate_url")]
    public string? SimplicateUrl { get; set; }
}

internal class Avatar
{
    [JsonPropertyName("url_small")]
    public string? UrlSmall { get; set; }

    [JsonPropertyName("url_large")]
    public string? UrlLarge { get; set; }

    [JsonPropertyName("initials")]
    public string? Initials { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }
}
