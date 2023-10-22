using System.Web;
using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {
        [MethodDescription("Invoices", "Search for invoices using multiple filters.")]
        public async Task<ChatGPTeamsAIClientResponse>? SearchInvoices(
            [ParameterDescription("The invoice number.")] string? invoiceNumber = null,
            [ParameterDescription("Organization name.")] string? organizationName = null,
            [ParameterDescription("My Organization profile id.")] string? myOrganizationProfileId = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? dateAfter = null,
            [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? dateBefore = null,
            [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            dateAfter?.EnsureValidDateFormat();
            dateBefore?.EnsureValidDateFormat();
            createdAfter?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(invoiceNumber)) filters["[invoice_number]"] = $"*{invoiceNumber}*";
            if (!string.IsNullOrEmpty(organizationName)) filters["[organization.name]"] = $"*{organizationName}*";
            if (!string.IsNullOrEmpty(myOrganizationProfileId)) filters["[my_organization_profile_id]"] = $"{myOrganizationProfileId}";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(dateAfter)) filters["[date][ge]"] = dateAfter;
            if (!string.IsNullOrEmpty(dateBefore)) filters["[date][le]"] = dateBefore;

            var result = await FetchSimplicateDataCollection<Invoice>(filters, "invoices/invoice", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Invoices", "Gets expired invoices using multiple filters.")]
        public async Task<ChatGPTeamsAIClientResponse> GetExpiredInvoices(
            [ParameterDescription("The invoice number.")] string? invoiceNumber = null,
            [ParameterDescription("Organization name.")] string? organizationName = null,
            [ParameterDescription("My Organization profile id.")] string? myOrganizationProfileId = null,
            [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? dateAfter = null,
            [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? dateBefore = null)
        {
            dateAfter?.EnsureValidDateFormat();
            dateBefore?.EnsureValidDateFormat();

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(invoiceNumber)) queryString["q[invoice_number]"] = $"*{invoiceNumber}*";
            if (!string.IsNullOrEmpty(organizationName)) queryString["q[organization.name]"] = $"*{organizationName}*";
            if (!string.IsNullOrEmpty(myOrganizationProfileId)) queryString["q[my_organization_profile_id]"] = $"{myOrganizationProfileId}";
            if (!string.IsNullOrEmpty(dateAfter)) queryString["q[date][ge]"] = dateAfter;
            if (!string.IsNullOrEmpty(dateBefore)) queryString["q[date][le]"] = dateBefore;

            var response = await _httpClient.PagedRequest<Invoice>($"invoices/invoice?{queryString}");

            var groupedHours = response
                .Where(a => a.Status.Name == "label_Expired");

            var result = new SimplicateDataCollectionResponse<Invoice>()
            {
                Data = groupedHours
            };

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Invoices", "Adds a new invoice to Simplicate.")]
        public async Task<NoOutputResponse> AddNewInvoice(
            [ParameterDescription("The payment term ID.")] string paymentTermId,
            [ParameterDescription("VAT class ID for the invoice line.")] string vatClassId,
            [ParameterDescription("Revenue group ID for the invoice line.")] string revenueGroupId,
            [ParameterDescription("Date for the invoice line.")] string date,
            [ParameterDescription("Description for the invoice line.")] string description,
            [ParameterDescription("Amount for the invoice line.")] double amount,
            [ParameterDescription("Price for the invoice line.")] double price,
            [ParameterDescription("Invoice status ID.")] string statusId,
            [ParameterDescription("My organization profile ID.")] string myOrganizationProfileId,
            [ParameterDescription("Organization ID.")] string organizationId,
            [ParameterDescription("Person ID.")] string personId,
            [ParameterDescription("Invoice date.")] string invoiceDate,
            [ParameterDescription("Invoice subject.")] string subject,
            [ParameterDescription("Invoice reference.")] string reference,
            [ParameterDescription("Project ID.")] string projectId,
            [ParameterDescription("Additional comments.")] string comments)
        {
            var invoiceLine = new
            {
                vat_class_id = vatClassId,
                revenue_group_id = revenueGroupId,
                date,
                description,
                amount,
                price
            };

            var invoiceData = new
            {
                payment_term_id = paymentTermId,
                invoice_lines = new[] { invoiceLine },
                status_id = statusId,
                my_organization_profile_id = myOrganizationProfileId,
                organization_id = organizationId,
                person_id = personId,
                date = invoiceDate,
                subject,
                reference,
                project_id = projectId,
                comments
            };

            var response = await _httpClient.PostAsync("invoices/invoice", invoiceData.PrepareJsonContent());

            if (response.IsSuccessStatusCode)
            {
                return SuccessResponse();
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("Invoices", "Fetches all payment terms from Simplicate.")]
        public async Task<ChatGPTeamsAIClientResponse>? GetPaymentTerms()
        {
            var response = await _httpClient.GetAsync("invoices/paymentterm");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.FromJson<SimplicateDataCollectionResponse<PaymentTerm>>();

                return ToChatGPTeamsAIResponse(result);
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("Invoices", "Fetches all VAT classes from Simplicate.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetVATClasses()
        {
            var response = await _httpClient.GetAsync("invoices/vatclass");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.FromJson<SimplicateDataCollectionResponse<VATClass>>();

                return ToChatGPTeamsAIResponse(result);
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("Invoices", "Gets all invoice statuses.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetAllInvoiceStatuses()
        {
            var response = await _httpClient.GetAsync("invoices/invoicestatus");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.FromJson<SimplicateDataCollectionResponse<InvoiceStatus>>();

                return ToChatGPTeamsAIResponse(result);
            }

            throw new Exception(response.ReasonPhrase);
        }
    }
}