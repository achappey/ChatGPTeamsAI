
namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;

internal class Invoice
{

    [JsonPropertyName("invoice_number")]
    [ListColumn]
    public string? InvoiceNumber { get; set; }

    [JsonPropertyName("date")]
    [ListColumn]
    public string? Date { get; set; }

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

    [JsonPropertyName("projectNamr")]
    [FormColumn]
    public string? ProjectName
    {
        get
        {
            return Project?.Name;
        }
        set { }
    }

    [JsonPropertyName("status")]
    public InvoiceStatus Status { get; set; } = null!;

    [JsonPropertyName("payment_term")]
    public PaymentTerm PaymentTerm { get; set; } = null!;

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? SimplicateUrl { get; set; } = null!;

    [JsonPropertyName("myOrganizationName")]
    [FormColumn]
    public string? MyOrganizationName
    {
        get
        {
            return MyOrganization?.Organization?.Name;
        }
        set { }
    }

    [FormColumn]
    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("project")]
    public ProjectInvoice? Project { get; set; } = null;

    [JsonPropertyName("organization")]
    public OrganizationInvoice? Organization { get; set; } = null!;

    [JsonPropertyName("my_organization_profile")]
    public MyOrganizationInvoice? MyOrganization { get; set; } = null!;

    [JsonPropertyName("comments")]
    [FormColumn]
    public string? Comments { get; set; }


}

internal class OrganizationInvoice
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class MyOrganizationInvoice
{
    [JsonPropertyName("organization")]
    public OrganizationInvoice? Organization { get; set; } = null!;
}

internal class InvoiceStatus
{

    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

internal class ProjectInvoice
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class PersonInvoice
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }

    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }
}

internal class PaymentTerm
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}
