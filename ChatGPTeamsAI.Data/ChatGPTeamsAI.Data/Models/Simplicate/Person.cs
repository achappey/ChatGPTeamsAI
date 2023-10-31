

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class Person
{

    [JsonPropertyName("full_name")]
    [ListColumn]
    public string? FullName { get; set; }

    [ListColumn]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [ListColumn]
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("initials")]
    [FormColumn]
    public string? Initials { get; set; }

    [JsonPropertyName("first_name")]
    [FormColumn]
    public string? FirstName { get; set; }

    [JsonPropertyName("gender")]
    [FormColumn]
    public string? Gender { get; set; }

    [JsonPropertyName("family_name")]
    [FormColumn]
    public string? FamilyName { get; set; }

    [JsonPropertyName("linkedin_url")]
    [LinkColumn]
    public string? LinkedIn { get; set; }

    [JsonPropertyName("website_url")]
    [LinkColumn]
    public string? Website { get; set; }

    [JsonPropertyName("relation_number")]
    [FormColumn]
    public string? RelationNumber { get; set; }



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

    [Ignore]
    [JsonPropertyName("relation_manager")]
    public RelationManager? RelationManager { get; set; }

    [JsonPropertyName("organizations")]
    [FormColumn]
    public string? Organizations
    {
        get
        {
            return LinkedAsContactToOrganization != null ? string.Join(", ", LinkedAsContactToOrganization.Select(a => a.Name)) : string.Empty;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("linked_as_contact_to_organization")]
    public IEnumerable<LinkedContactPerson>? LinkedAsContactToOrganization { get; set; }
}

internal class LinkedContactPerson
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("work_function")]
    public string? WorkFunction { get; set; }
}