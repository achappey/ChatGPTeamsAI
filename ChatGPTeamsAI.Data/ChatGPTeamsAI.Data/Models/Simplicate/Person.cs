

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class Person
{

    [JsonPropertyName("full_name")]
    [ListColumn]
    [FormColumn("General")]
    [NewFormColumn]
    [TitleColumn]
    public string? FullName { get; set; }

    [JsonPropertyName("image")]
    [Ignore]
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

    [JsonPropertyName("gender")]
    [FormColumn("General")]
    public string? Gender { get; set; }

    [JsonPropertyName("first_name")]
    [NewFormColumn]
    [FormColumn("General")]
    public string? FirstName { get; set; }

    [JsonPropertyName("initials")]
    [NewFormColumn]
    [FormColumn("General")]
    public string? Initials { get; set; }

    [JsonPropertyName("family_name")]
    [NewFormColumn]
    [FormColumn("General")]
    public string? FamilyName { get; set; }

    [FormColumn("Contact")]
    [NewFormColumn]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [NewFormColumn]
    [FormColumn("Contact")]
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("linkedin_url")]
    [NewFormColumn]
    [LinkColumn(category: "Contact")]
    public string? LinkedIn { get; set; }

    [JsonPropertyName("website_url")]
    [NewFormColumn]
    [LinkColumn(category: "Contact")]
    public string? Website { get; set; }

    [JsonPropertyName("relation_number")]
    [FormColumn("Extra")]
    public string? RelationNumber { get; set; }

    [JsonPropertyName("relation_type")]
    [Ignore]
    public RelationType? RelationType { get; set; }

    [JsonPropertyName("relationTypeLabel")]
    [FormColumn("General")]
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
    [FormColumn("Address")]
    public string? AddressLocality
    {
        get
        {
            return Address?.Locality;
        }
        set { }
    }

    [JsonPropertyName("relationManagerName")]
    [FormColumn("Extra")]
    public string? RelationManagerName
    {
        get
        {
            return RelationManager?.Name;
        }
        set { }
    }

    [Ignore]
    [LinkColumn(category: "Contact")]
    public string? SendMail
    {
        get
        {
            return !string.IsNullOrEmpty(Email) ? $"mailto:{Email}" : null;
        }
        set { }
    }

    [Ignore]
    [LinkColumn(category: "Contact")]
    public string? Call
    {
        get
        {
            return !string.IsNullOrEmpty(Phone) ? $"tel:{Phone}" : null;
        }
        set { }
    }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn(category: "General")]
    public string? Simplicate { get; set; }

    [Ignore]
    [JsonPropertyName("relation_manager")]
    public RelationManager? RelationManager { get; set; }

    [JsonPropertyName("organizations")]
    [FormColumn("ContactAt")]
    public string? Organizations
    {
        get
        {
            if (LinkedAsContactToOrganization != null && LinkedAsContactToOrganization.Any())
            {
                return "- " + string.Join("\r- ", LinkedAsContactToOrganization.Select(a => a.ToString()));
            }
            return string.Empty;
        }
        set { }
    }

    [JsonPropertyName("teamNames")]
    [FormColumn("Extra")]
    public string? TeamNames
    {
        get
        {
            if (Teams != null && Teams.Any())
            {
                return "- " + string.Join("\r- ", Teams.Select(a => a.Name));
            }
            return string.Empty;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("linked_as_contact_to_organization")]
    public IEnumerable<LinkedContactPerson>? LinkedAsContactToOrganization { get; set; }

    [Ignore]
    [JsonPropertyName("teams")]
    public IEnumerable<Team>? Teams { get; set; }

    [JsonPropertyName("note")]
    [NewFormColumn]
    [FormColumn("General")]
    public string? Note { get; set; }

    [JsonPropertyName("created_at")]
    [FormColumn("General")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn("General")]
    [UpdatedColumn]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("edit")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? Edit
    {
        get { return Id != null ? new Dictionary<string, object>() { { "personId", Id } } : null; }
        set { }
    }

    [JsonPropertyName("newOrganization")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? NewOrganization
    {
        get { return Id != null && (LinkedAsContactToOrganization == null || !LinkedAsContactToOrganization.Any()) ? new Dictionary<string, object>() { { "personId", Id } } : null; }
        set { }
    }

    [JsonPropertyName("newSales")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? NewSales
    {
        get { return Id != null ? new Dictionary<string, object>() { { "personId", Id } } : null; }
        set { }
    }

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


internal class Team
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

}