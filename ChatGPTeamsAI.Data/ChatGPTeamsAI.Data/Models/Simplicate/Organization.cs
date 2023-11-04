
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
    [FormColumn]
    public string? Email { get; set; }

    [JsonPropertyName("logo")]
    [ImageColumn]
    public string? Logo
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

    [JsonPropertyName("phone")]
    [FormColumn]
    public string? Phone { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("coc_code")]
    [FormColumn]
    public string? CocCode { get; set; }

    [JsonPropertyName("vat_number")]
    [FormColumn]
    public string? VatNumber { get; set; }

    [JsonPropertyName("is_active")]
    [FormColumn]
    public bool IsActive { get; set; }

    [JsonPropertyName("url")]
    [LinkColumn]
    public string? Website { get; set; }

    [JsonPropertyName("linkedin_url")]
    [LinkColumn]
    public string? LinkedIn { get; set; }

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
    [FormColumn]
    public string? DebtorMail
    {
        get
        {
            return Debtor?.SendEmailEmail;
        }
        set { }
    }

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

    [JsonPropertyName("interests")]
    [Ignore]
    public IEnumerable<Interest>? Interests { get; set; }

    [JsonPropertyName("InteresNames")]
    [FormColumn]
    public string? InterestsNames
    {
        get
        {
            return Interests != null ? string.Join(", ", Interests.Select(a => a.Name)) : string.Empty;
        }
        set { }
    }

    [JsonPropertyName("created_at")]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn]
    [UpdatedColumn]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("note")]
    [FormColumn]
    public string? Note { get; set; }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? Simplicate { get; set; }

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
    [FormColumn]
    [ListColumn]
    public string? Name { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("vat_number")]
    [FormColumn]
    public string? VatNumber { get; set; }

    [JsonPropertyName("coc_code")]
    [FormColumn]
    [ListColumn]
    public string? CocCode { get; set; }

    [FormColumn]
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