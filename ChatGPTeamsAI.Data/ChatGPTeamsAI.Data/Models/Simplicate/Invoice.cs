
#nullable enable

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

  //  [JsonPropertyName("project_id")]
 //   public string? ProjectId { get; set; }

   /* [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("my_organization_profile_id")]
    public string? MyOrganizationProfileId { get; set; }  = null!;*/

   // [JsonPropertyNameName("total_excluding_vat")]
   // public decimal TotalExcludingVat { get; set; }

  //  [JsonPropertyNameName("total_outstanding")]
  //  public decimal TotalOutstanding { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("project")]
    public ProjectInvoice? Project { get; set; } = null;

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

internal class OrganizationInvoice
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class PaymentTerm
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}
