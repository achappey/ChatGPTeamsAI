namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Invoice
{

    [JsonPropertyName("invoice_number")]
    [ListColumn]
    public string? InvoiceNumber { get; set; }

    [JsonPropertyName("date")]
    [ListColumn]
    public string? Date { get; set; }

    [FormColumn]
    [ListColumn]
    [JsonPropertyName("organizationName")]
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

    [JsonPropertyName("statusName")]
    [FormColumn]
    public string? StatusName
    {
        get
        {
            return Status?.Name;
        }
        set { }
    }

    [JsonPropertyName("status")]
    [Ignore]
    public InvoiceStatus Status { get; set; } = null!;

    [Ignore]
    [JsonPropertyName("payment_term")]
    public PaymentTerm PaymentTerm { get; set; } = null!;

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? Simplicate { get; set; } = null!;

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

    [Ignore]
    [JsonPropertyName("project")]
    public ProjectInvoice? Project { get; set; } = null;

    [Ignore]
    [JsonPropertyName("organization")]
    public OrganizationInvoice? Organization { get; set; } = null!;

    [Ignore]
    [JsonPropertyName("my_organization_profile")]
    public MyOrganizationInvoice? MyOrganization { get; set; } = null!;

    [JsonPropertyName("comments")]
    [FormColumn]
    public string? Comments { get; set; }

    [JsonPropertyName("total_excluding_vat")]
    [FormColumn]
    public double? TotalExcludingVat { get; set; }

    [JsonPropertyName("getProject")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetProject
    {
        get { return Project != null ? new Dictionary<string, object?>() { { "projectId", Project.Id } } : null; }
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

internal class OrganizationInvoice
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

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
    [ListColumn]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

internal class ProjectInvoice
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

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
    [ListColumn]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

internal class InvoiceDocument : Document
{

}

internal class Document
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("download_url")]
    [LinkColumn]
    public string? Open { get; set; }

    [JsonPropertyName("document_type")]
    [Ignore]
    public DocumentType? DocumentType { get; set; }

    [JsonPropertyName("linked_to")]
    [Ignore]
    public List<LinkedTo>? LinkedTo { get; set; }

    [JsonPropertyName("created_by")]
    [Ignore]
    public CreatedBy? CreatedBy { get; set; }

    [FormColumn]
    [JsonPropertyName("createdByName")]
    public string? CreatedByName
    {
        get
        {
            return CreatedBy?.Name;
        }
        set { }
    }

    [FormColumn]
    [JsonPropertyName("documentTypeLabel")]
    public string? DocumentTypeLabel
    {
        get
        {
            return DocumentType?.Label;
        }
        set { }
    }

    [JsonPropertyName("created_at")]
    [ListColumn]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("title")]
    [FormColumn]
    [ListColumn]
    public string? Filename { get; set; }


    [JsonPropertyName("linkedToLabels")]
    [FormColumn]
    public string? LinkedToLabels
    {
        get
        {
            return LinkedTo != null ? string.Join(", ", LinkedTo.Select(a => a.Label)) : string.Empty;
        }
        set { }
    }

    private string? _description;

    [JsonPropertyName("description")]
    [FormColumn]
    public string? Description
    {
        get
        {
            if (_description != null)
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(_description);
                return htmlDoc.DocumentNode.InnerText;
            }
            return null;
        }
        set
        {
            _description = value;
        }
    }


}

internal class DocumentType
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

internal class CreatedBy
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
