
using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class Organization
{
    [JsonPropertyName("name")]
    [ListColumn]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    [ListColumn]
    public string? Email { get; set; }

    [JsonPropertyName("phone")]
    [ListColumn]
    public string? Phone { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("coc_code")]
    [FormColumn]
    public string? CocCode { get; set; }

    [JsonPropertyName("is_active")]
    [FormColumn]
    public bool IsActive { get; set; }

    [JsonPropertyName("relation_manager")]
    [Ignore]
    public RelationManager? RelationManager { get; set; }

    [JsonPropertyName("relationManagerName")]
    [FormColumn]
    public string? RelationManagerName
    {
        get
        {
            return RelationManager?.Name;
        }
        set { }
    }

    [JsonPropertyName("industry")]
    [Ignore]
    public Industry? Industry { get; set; }

    [JsonPropertyName("industryName")]
    [FormColumn]
    public string? IndustryName
    {
        get
        {
            return Industry?.Name;
        }
        set { }
    }

    [LinkColumn]
    public string? SendMail
    {
        get
        {
            return !string.IsNullOrEmpty(Email) ? $"mailto:{Email}" : null;
        }
        set { }
    }

    [LinkColumn]
    public string? Call
    {
        get
        {
            return !string.IsNullOrEmpty(Phone) ? $"tel:{Phone}" : null;
        }
        set { }
    }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? Simplicate { get; set; }

}

internal class RelationManager
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class Industry
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    [ListColumn]
    public string? Name { get; set; }
}

internal class MyOrganizationProfile
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("vat_number")]
    public string? VatNumber { get; set; }

    [JsonPropertyName("coc_code")]
    public string? CocCode { get; set; }

    [JsonPropertyName("bank_account")]
    public string? BankAccount { get; set; }

}


internal class RelationType
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [ListColumn]
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("color")]
    [ListColumn]
    public string? Color { get; set; }

    [JsonPropertyName("type")]
    [ListColumn]
    public string? Type { get; set; }
}