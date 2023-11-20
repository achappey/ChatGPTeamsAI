
using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class Organization
{
    [JsonPropertyName("name")]
    [TitleColumn]
    [ListColumn]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    [FormColumn("General")]
    public string? Email { get; set; }

    [JsonPropertyName("logo")]
    [Ignore]
    [ImageColumn]
    public string? Logo
    {
        get
        {
            string? emailToUse = Email;

            if (string.IsNullOrEmpty(emailToUse) || !emailToUse.Contains('@'))
            {
                emailToUse = DebtorMail;
            }

            if (string.IsNullOrEmpty(emailToUse) || !emailToUse.Contains('@'))
            {
                return string.Empty;
            }

            var domain = emailToUse.Split('@')[1];

            return $"https://logo.clearbit.com/{domain}";
        }
    }

    [JsonPropertyName("phone")]
    [FormColumn("General")]
    public string? Phone { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("coc_code")]
    [FormColumn("General")]
    public string? CocCode { get; set; }

    [JsonPropertyName("vat_number")]
    [FormColumn("Invoicing")]
    public string? VatNumber { get; set; }

    [JsonPropertyName("is_active")]
    [FormColumn("General")]
    public bool IsActive { get; set; }

    [JsonPropertyName("url")]
    [LinkColumn(category: "General")]
    public string? Website { get; set; }

    [JsonPropertyName("linkedin_url")]
    [LinkColumn(category: "General")]
    public string? LinkedIn { get; set; }

    [JsonPropertyName("relation_manager")]
    [Ignore]
    public RelationManager? RelationManager { get; set; }

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

    [JsonPropertyName("industry")]
    [Ignore]
    public Industry? Industry { get; set; }

    [JsonPropertyName("relation_type")]
    [Ignore]
    public RelationType? RelationType { get; set; }

    [JsonPropertyName("debtor")]
    [Ignore]
    public Debtor? Debtor { get; set; }

    [JsonPropertyName("visiting_address")]
    [Ignore]
    public Address? VisitingAddress { get; set; }

    [JsonPropertyName("visitingAddressLocality")]
    [ListColumn]
    public string? VisitingAddressLocality
    {
        get
        {
            return VisitingAddress?.Locality;
        }
        set { }
    }

    [JsonPropertyName("debtorEmail")]
    [FormColumn("Invoicing")]
    public string? DebtorMail
    {
        get
        {
            return Debtor?.SendEmailEmail;
        }
        set { }
    }

    [JsonPropertyName("industryName")]
    [FormColumn("Extra")]
    public string? IndustryName
    {
        get
        {
            return Industry?.Name;
        }
        set { }
    }

    [Ignore]
    [LinkColumn(category: "General")]
    public string? SendMail
    {
        get
        {
            return !string.IsNullOrEmpty(Email) ? $"mailto:{Email}" : null;
        }
        set { }
    }

    [Ignore]
    [LinkColumn(category: "General")]
    public string? Call
    {
        get
        {
            return !string.IsNullOrEmpty(Phone) ? $"tel:{Phone}" : null;
        }
        set { }
    }

    [JsonPropertyName("interests")]
    [Ignore]
    public IEnumerable<Interest>? Interests { get; set; }

    [JsonPropertyName("teamNames")]
    [FormColumn("Extra")]
    public string? TeamNames
    {
        get
        {
            if (Teams != null && Teams.Any())
            {
                return string.Join(", ", Teams.Select(a => a.Name));
            }
            return string.Empty;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("teams")]
    public IEnumerable<Team>? Teams { get; set; }

    [JsonPropertyName("created_at")]
    [FormColumn("General")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn("General")]
    [UpdatedColumn]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("note")]
    [FormColumn("General")]
    public string? Note { get; set; }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn(category: "General")]
    public string? Simplicate { get; set; }

    [JsonPropertyName("newPerson")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? NewPerson
    {
        get { return Id != null ? new Dictionary<string, object>() { { "organizationId", Id } } : null; }
        set { }
    }

    [JsonPropertyName("newSales")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object>? NewSales
    {
        get { return Id != null ? new Dictionary<string, object>() { { "organizationId", Id } } : null; }
        set { }
    }
}

internal class Address
{
    [JsonPropertyName("locality")]
    public string? Locality { get; set; }
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

internal class Interest
{
    [JsonPropertyName("name")]
    [ListColumn]
    public string? Name { get; set; }
}


internal class Debtor
{
    [JsonPropertyName("send_email_email")]
    [ListColumn]
    public string? SendEmailEmail { get; set; }
}

internal class MyOrganizationProfile
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    [FormColumn("General")]
    [ListColumn]
    public string? Name { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("vat_number")]
    [FormColumn("Invoicing")]
    public string? VatNumber { get; set; }

    [JsonPropertyName("coc_code")]
    [FormColumn("Invoicing")]
    [ListColumn]
    public string? CocCode { get; set; }

    [FormColumn("Invoicing")]
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