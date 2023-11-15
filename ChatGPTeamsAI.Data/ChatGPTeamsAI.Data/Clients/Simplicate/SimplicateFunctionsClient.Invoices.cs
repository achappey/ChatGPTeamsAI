using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {
        [MethodDescription("Invoices", "Search for invoice payments", "ExportPayments")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchPayments(
                   [ParameterDescription("Description")] string? description = null,
                   [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateAfter = null,
                   [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateBefore = null,
                   [ParameterDescription("Page number")] long pageNumber = 1)
        {
            dateAfter?.EnsureValidDateFormat();
            dateBefore?.EnsureValidDateFormat();

            var filters = CreatePaymentFilters(description, dateAfter, dateBefore);

            var result = await FetchSimplicateDataCollection<Payment>(filters, "invoices/payment", pageNumber, "-date");

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of invoice payments")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportPayments(
            [ParameterDescription("Description")] string? description = null,
                   [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateAfter = null,
                   [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateBefore = null)
        {
            dateAfter?.EnsureValidDateFormat();
            dateBefore?.EnsureValidDateFormat();

            var filters = CreatePaymentFilters(description, dateAfter, dateBefore);
            var queryString = BuildQueryString(filters, "-date");
            var response = await _httpClient.PagedRequest<Payment>($"invoices/payment?{queryString}");

            var result = new SimplicateDataCollectionResponse<Payment>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreatePaymentFilters(
            string? description,
            string? dateAfter,
            string? dateBefore)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(description)) filters["[description]"] = $"*{description}*";
            if (!string.IsNullOrEmpty(dateAfter)) filters["[date][ge]"] = dateAfter;
            if (!string.IsNullOrEmpty(dateBefore)) filters["[date][le]"] = dateBefore;
            return filters;
        }

        [MethodDescription("Invoices", "Search for invoice documents")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchInvoiceDocuments(
                  [ParameterDescription("Name of employee who created the document")] string? createdBy = null,
                  [ParameterDescription("Title of the document")] string? title = null,
                  [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
                  [ParameterDescription("Created at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdBefore = null,
                  [ParameterDescription("Page number")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();
            createdBefore?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(title)) filters["[title]"] = $"*{title}*";
            if (!string.IsNullOrEmpty(createdBy)) filters["[created_by.name]"] = $"*{createdBy}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(createdBefore)) filters["[created_at][le]"] = createdBefore;

            var result = await FetchSimplicateDataCollection<InvoiceDocument>(filters, "invoices/document", pageNumber, "-created_at");

            return ToChatGPTeamsAIResponse(result);
        }


        [MethodDescription("Invoices", "Search for invoices", "ExportInvoices")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchInvoices(
            [ParameterDescription("Invoice number")] string? invoiceNumber = null,
            [ParameterDescription("Name of the invoice company")] string? organizationName = null,
            [ParameterDescription("Name of the invoicer company")] string? myOrganizationProfileName = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
            [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateAfter = null,
            [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateBefore = null,
            [ParameterDescription("Page number")] long pageNumber = 1)
        {
            ValidateInvoiceFilters(createdAfter, dateAfter, dateBefore);

            var filters = CreateInvoiceFilters(invoiceNumber, organizationName, myOrganizationProfileName, createdAfter, dateAfter, dateBefore);
            var result = await FetchSimplicateDataCollection<Invoice>(filters, "invoices/invoice", pageNumber, "-date");

            return ToChatGPTeamsAIResponse(result);
        }


        [MethodDescription("Export", "Exports a list of invoices")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportInvoices(
            [ParameterDescription("Invoice number")] string? invoiceNumber = null,
            [ParameterDescription("Name of the invoice company")] string? organizationName = null,
            [ParameterDescription("Name of the invoicer company")] string? myOrganizationProfileName = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
            [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateAfter = null,
            [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateBefore = null)
        {
            ValidateInvoiceFilters(createdAfter, dateAfter, dateBefore);

            var filters = CreateInvoiceFilters(invoiceNumber, organizationName, myOrganizationProfileName, createdAfter, dateAfter, dateBefore);
            var queryString = BuildQueryString(filters, "-date");
            var response = await _httpClient.PagedRequest<Invoice>($"invoices/invoice?{queryString}");

            var result = new SimplicateDataCollectionResponse<Invoice>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreateInvoiceFilters(
            string? invoiceNumber,
            string? organizationName,
            string? myOrganizationProfileName,
            string? createdAfter,
            string? dateAfter,
            string? dateBefore)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(invoiceNumber)) filters["[invoice_number]"] = $"*{invoiceNumber}*";
            if (!string.IsNullOrEmpty(organizationName)) filters["[organization.name]"] = $"*{organizationName}*";
            if (!string.IsNullOrEmpty(myOrganizationProfileName)) filters["[my_organization_profile.organization.name]"] = $"*{myOrganizationProfileName}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(dateAfter)) filters["[date][ge]"] = dateAfter;
            if (!string.IsNullOrEmpty(dateBefore)) filters["[date][le]"] = dateBefore;
            return filters;
        }

        [MethodDescription("Invoices", "Get total invoices per my organization/invoicer")]
        public async Task<ChatGPTeamsAIClientResponse?> GetTotalExcludingVatPerMyOrganization(
            [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string dateAfter,
            [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string dateBefore)
        {
            dateAfter?.EnsureValidDateFormat();
            dateBefore?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(dateAfter)) filters["[date][ge]"] = dateAfter;
            if (!string.IsNullOrEmpty(dateBefore)) filters["[date][le]"] = dateBefore;

            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<Invoice>($"invoices/invoice?{queryString}");

            var items = response.Where(t => t.TotalExcludingVat.HasValue).GroupBy(t => t.MyOrganizationName).Select(a => new Summary()
            {
                Description = a.Key,
                Total = Math.Round(a.Sum(s => s.TotalExcludingVat!.Value))
            });

            var result = new SimplicateDataCollectionResponse<Summary>()
            {
                Data = items
            };

            return ToChatGPTeamsAIResponse(result);
        }

        private void ValidateInvoiceFilters(
          string? createdAfter,
          string? dateAfter,
          string? dateBefore)
        {
            dateAfter?.EnsureValidDateFormat();
            dateBefore?.EnsureValidDateFormat();
            createdAfter?.EnsureValidDateFormat();
        }
        /*
                [MethodDescription("Invoices", "Gets expired invoices using multiple filters")]
                public async Task<ChatGPTeamsAIClientResponse?> GetExpiredInvoices(
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
                }*/

        [MethodDescription("Invoices", "Search for invoice propositions")]
        public async Task<ChatGPTeamsAIClientResponse?> GetPropositions(
            [ParameterDescription("Project name")] string? projectName = null,
            [ParameterDescription("Project number")] string? projectNumber = null,
            [ParameterDescription("Projectmanager")] string? projectManager = null,
            [ParameterDescription("Organization name")] string? organizationName = null,
            [ParameterDescription("Pagenumber")] long pageNumber = 1)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(projectName)) filters["[project.name]"] = $"*{projectName}*";
            if (!string.IsNullOrEmpty(projectNumber)) filters["[project.project_number]"] = $"*{projectNumber}*";
            if (!string.IsNullOrEmpty(projectManager)) filters["[project.project_manager.name]"] = $"*{projectManager}*";
            if (!string.IsNullOrEmpty(organizationName)) filters["[project.organization.name]"] = $"*{organizationName}*";

            var result = await FetchSimplicateDataCollection<Proposition>(filters, "invoices/proposition", pageNumber);
            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Invoices", "Adds a new invoice to Simplicate")]
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
        public async Task<ChatGPTeamsAIClientResponse?> GetPaymentTerms()
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