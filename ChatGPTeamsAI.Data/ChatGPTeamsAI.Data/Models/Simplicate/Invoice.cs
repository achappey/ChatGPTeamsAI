
namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;

internal class Invoice
{

    [JsonPropertyName("invoice_number")]
    public string? InvoiceNumber { get; set; } 

    [JsonPropertyName("comments")]
    public string? Comments { get; set; }

     [JsonPropertyName("status")]
    public InvoiceStatus Status { get; set; } = null!;

    [JsonPropertyName("payment_term")]
    public PaymentTerm PaymentTerm { get; set; }  = null!;

    [JsonPropertyName("simplicate_url")]
    public string? SimplicateUrl { get; set; }  = null!;

    [JsonPropertyName("date")]
    public string? Date { get; set; } 

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("project")]
    public ProjectInvoice? Project { get; set; } = null;

    [JsonPropertyName("organization")]
    public OrganizationInvoice? Organization { get; set; }  = null!;

    [JsonPropertyName("my_organization_profile")]
    public MyOrganizationInvoice? MyOrganization { get; set; }  = null!;

}

internal class OrganizationInvoice
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class MyOrganizationInvoice
{
    [JsonPropertyName("organization")]
    public OrganizationInvoice? Organization { get; set; }  = null!;
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
