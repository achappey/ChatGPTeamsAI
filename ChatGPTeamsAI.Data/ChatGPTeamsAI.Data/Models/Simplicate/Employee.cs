namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Employee
{
    [JsonPropertyName("name")]
    [ListColumn]
    [TitleColumn]
    public string? Name { get; set; }

    [JsonPropertyName("image")]
    [ImageColumn]
    public string? Image
    {
        get
        {
            return Avatar?.UrlLarge;
        }
    }

    [JsonPropertyName("function")]
    public string? Function { get; set; }

    [JsonPropertyName("work_email")]
    public string? WorkEmail { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("person_id")]
    public string? PersonId { get; set; }

    [JsonPropertyName("employment_status")]
    [FormColumn]
    public string? EmploymentStatus { get; set; }

    [JsonPropertyName("CivilStatusLabel")]
    [FormColumn]
    public string? CivilStatusLabel
    {
        get
        {
            return CivilStatus?.Label;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("civil_status")]
    public CivilStatus? CivilStatus { get; set; }

    [JsonPropertyName("work_phone")]
    [FormColumn]
    public string? WorkPhone { get; set; }

    [JsonPropertyName("work_mobile")]
    [FormColumn]
    public string? WorkMobile { get; set; }

    [JsonPropertyName("hourly_sales_tariff")]
    [FormColumn]
    public string? HourlySalesTariff { get; set; }

    [JsonPropertyName("hourly_cost_tariff")]
    [FormColumn]
    public string? HourlyCostTariff { get; set; }

    [JsonPropertyName("created_at")]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn]
    [UpdatedColumn]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? Simplicate { get; set; }

    [JsonPropertyName("avatar")]
    [Ignore]
    public Avatar? Avatar { get; set; }
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

internal class CivilStatus
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}