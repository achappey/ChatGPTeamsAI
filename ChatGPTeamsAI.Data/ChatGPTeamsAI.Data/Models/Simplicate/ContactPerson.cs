using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class ContactPerson
{
    [JsonPropertyName("personFullName")]
    [ListColumn]
    public string? PersonName
    {
        get
        {
            return Person?.FullName;
        }
        set { }
    }

    [JsonPropertyName("organizationName")]
    [ListColumn]
    public string? OrganizationName
    {
        get
        {
            return Organization?.Name;
        }
        set { }
    }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [Ignore]
    [JsonPropertyName("organization")]
    public OrganizationContactPerson? Organization { get; set; }

    [JsonPropertyName("person")]
    [Ignore]
    public PersonContactPerson? Person { get; set; }

    [JsonPropertyName("is_active")]
    [FormColumn]
    public bool IsActive { get; set; }

    [JsonPropertyName("work_function")]
    [ListColumn]
    public string? WorkFunction { get; set; }

    [FormColumn]
    [JsonPropertyName("work_email")]
    public string? WorkEmail { get; set; }

    [JsonPropertyName("work_phone")]
    [FormColumn]
    public string? WorkPhone { get; set; }

    [JsonPropertyName("work_mobile")]
    [FormColumn]
    public string? WorkMobile { get; set; }

    [JsonPropertyName("created_at")]
    [Ignore]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [Ignore]
    [FormColumn]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("getPerson")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetPerson
    {
        get { return Person != null ? new Dictionary<string, object?>() { { "personId", Person.Id } } : null; }
        set { }
    }

    [JsonPropertyName("getOrganization")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetOrganization
    {
        get { return Organization != null ? new Dictionary<string, object?>() { { "organizationId", Organization.Id } } : null; }
        set { }
    }
}

internal class OrganizationContactPerson
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }
}

internal class PersonContactPerson
{
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

}
