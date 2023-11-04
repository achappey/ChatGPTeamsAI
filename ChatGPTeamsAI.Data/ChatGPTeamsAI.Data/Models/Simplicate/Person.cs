

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class Person
{

    [JsonPropertyName("full_name")]
    [ListColumn]
    [TitleColumn]
    public string? FullName { get; set; }

    [JsonPropertyName("image")]
    [ImageColumn]
    public string? Image
    {
        get
        {
            if (string.IsNullOrEmpty(Email) || !Email.Contains('@'))
            {
                return string.Empty;
            }

            var domain = Email.Split('@')[1];

            return $"https://logo.clearbit.com/{domain}";
        }
    }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("initials")]
    [FormColumn]
    public string? Initials { get; set; }

    [JsonPropertyName("first_name")]
    [FormColumn]
    public string? FirstName { get; set; }

    [JsonPropertyName("family_name")]
    [FormColumn]
    public string? FamilyName { get; set; }

    [JsonPropertyName("gender")]
    [FormColumn]
    public string? Gender { get; set; }

    [FormColumn]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [FormColumn]
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("linkedin_url")]
    [LinkColumn]
    public string? LinkedIn { get; set; }

    [JsonPropertyName("website_url")]
    [LinkColumn]
    public string? Website { get; set; }

    [JsonPropertyName("relation_number")]
    [FormColumn]
    public string? RelationNumber { get; set; }

    [JsonPropertyName("relation_type")]
    [Ignore]
    public RelationType? RelationType { get; set; }

    [JsonPropertyName("relationTypeLabel")]
    [FormColumn]
    public string? RelationTypeLabel
    {
        get
        {
            return RelationType?.Label;
        }
        set { }
    }

    [JsonPropertyName("address")]
    [Ignore]
    public Address? Address { get; set; }

    [JsonPropertyName("addressLocality")]
    [FormColumn]
    public string? AddressLocality
    {
        get
        {
            return Address?.Locality;
        }
        set { }
    }

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
    [ListColumn]
    public string? Organizations
    {
        get
        {
            return LinkedAsContactToOrganization != null
           ? string.Join(", ", LinkedAsContactToOrganization.Select(a => a.ToString()))
           : string.Empty;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("linked_as_contact_to_organization")]
    public IEnumerable<LinkedContactPerson>? LinkedAsContactToOrganization { get; set; }

    [JsonPropertyName("created_at")]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn]
    [UpdatedColumn]
    public string? UpdatedAt { get; set; }
}

internal class LinkedContactPerson
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("work_function")]
    public string? WorkFunction { get; set; }

    public override string ToString()
    {
        return $"{Name}{(string.IsNullOrEmpty(WorkFunction) ? string.Empty : $" ({WorkFunction})")}";
    }
}