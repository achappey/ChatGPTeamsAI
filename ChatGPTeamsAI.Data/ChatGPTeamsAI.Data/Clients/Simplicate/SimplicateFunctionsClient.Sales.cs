using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {

        [MethodDescription("Sales", "Search for sales", "ExportSales")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchSales(
            [ParameterDescription("The name of the responsible employee")] string? responsibleEmployeeName = null,
            [ParameterDescription("Organization name")] string? organizationName = null,
            [ParameterDescription("Person name")] string? personName = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
            [ParameterDescription("Sales subject")] string? subject = null,
            [ParameterDescription("The page number")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreateSalesFilters(responsibleEmployeeName, organizationName, personName, createdAfter, subject);
            var result = await FetchSimplicateDataCollection<Sales>(filters, "sales/sales", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of sales data")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportSales(
            [ParameterDescription("The name of the responsible employee")] string? responsibleEmployeeName = null,
            [ParameterDescription("Organization name")] string? organizationName = null,
            [ParameterDescription("Person name")] string? personName = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
            [ParameterDescription("Sales subject")] string? subject = null)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreateSalesFilters(responsibleEmployeeName, organizationName, personName, createdAfter, subject);
            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<Sales>($"sales/sales?{queryString}");

            var result = new SimplicateDataCollectionResponse<Sales>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Sales", "Search for quotes", "ExportQuotes")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchQuotes(
            [ParameterDescription("Quote number")] string? quoteNumber = null,
            [ParameterDescription("Status label")] string? statusLabel = null,
            [ParameterDescription("Quote subject")] string? quoteSubject = null,
            [ParameterDescription("Quote date at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? quoteDateAfter = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
            [ParameterDescription("Customer reference")] string? customerReference = null,
            [ParameterDescription("The page number")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();
            quoteDateAfter?.EnsureValidDateFormat();

            var filters = CreateQuotesFilters(quoteNumber, statusLabel, quoteSubject, quoteDateAfter, createdAfter, customerReference);

            var result = await FetchSimplicateDataCollection<Quote>(filters, "sales/quote", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of quotes data")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportQuotes(
            [ParameterDescription("Quote number")] string? quoteNumber = null,
            [ParameterDescription("Status label")] string? statusLabel = null,
            [ParameterDescription("Quote subject")] string? quoteSubject = null,
            [ParameterDescription("Quote date at or after")] string? quoteDateAfter = null,
            [ParameterDescription("Created at or after")] string? createdAfter = null,
            [ParameterDescription("Customer reference")] string? customerReference = null)
        {
            createdAfter?.EnsureValidDateFormat();
            quoteDateAfter?.EnsureValidDateFormat();

            var filters = CreateQuotesFilters(quoteNumber, statusLabel, quoteSubject, quoteDateAfter, createdAfter, customerReference);
            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<Quote>($"sales/quote?{queryString}");

            var result = new SimplicateDataCollectionResponse<Quote>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreateQuotesFilters(
            string? quoteNumber,
            string? statusLabel,
            string? quoteSubject,
            string? quoteDateAfter,
            string? createdAfter,
            string? customerReference)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(quoteNumber)) filters["[quote_number]"] = $"*{quoteNumber}*";
            if (!string.IsNullOrEmpty(statusLabel)) filters["[quotestatus.label]"] = $"*{statusLabel}*";
            if (!string.IsNullOrEmpty(quoteSubject)) filters["[quote_subject]"] = $"*{quoteSubject}*";
            if (!string.IsNullOrEmpty(quoteDateAfter)) filters["[quote_date][ge]"] = quoteDateAfter;
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(customerReference)) filters["[customer_reference]"] = $"*{customerReference}*";

            return filters;
        }


        private Dictionary<string, string> CreateSalesFilters(
                string? responsibleEmployeeName,
                string? organizationName,
                string? personName,
                string? createdAfter,
                string? subject)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(responsibleEmployeeName)) filters["[responsible_employee.name]"] = $"*{responsibleEmployeeName}*";
            if (!string.IsNullOrEmpty(organizationName)) filters["[organization.name]"] = $"*{organizationName}*";
            if (!string.IsNullOrEmpty(personName)) filters["[person.full_name]"] = $"*{personName}*";
            if (!string.IsNullOrEmpty(subject)) filters["[subject]"] = $"*{subject}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;

            return filters;
        }


        [MethodDescription("Sales", "Fetches all revenue groups")]
        public async Task<ChatGPTeamsAIClientResponse?> GetAllRevenueGroups()
        {
            var response = await _httpClient.GetAsync("sales/revenuegroup");

            var result = await response.FromJson<SimplicateDataCollectionResponse<RevenueGroup>?>();

            return ToChatGPTeamsAIResponse(result);
        }
    }

}