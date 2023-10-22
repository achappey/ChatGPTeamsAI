using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {

        [MethodDescription("Sales", "Search for sales using multiple filters.")]
        public async Task<SimplicateDataCollectionResponse<Sales>?>? SearchSales(
            [ParameterDescription("The name of the responsible employee.")] string? responsibleEmployeeName = null,
            [ParameterDescription("Organization name.")] string? organizationName = null,
            [ParameterDescription("Person name.")] string? personName = null,
            [ParameterDescription("Sales subject.")] string? subject = null,
            [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(responsibleEmployeeName)) filters["[responsible_employee.name]"] = $"*{responsibleEmployeeName}*";
            if (!string.IsNullOrEmpty(organizationName)) filters["[organization.name]"] = $"*{organizationName}*";
            if (!string.IsNullOrEmpty(personName)) filters["[person.full_name]"] = $"*{personName}*";
            if (!string.IsNullOrEmpty(subject)) filters["[subject]"] = $"*{subject}*";

            return await FetchSimplicateDataCollection<Sales>(filters, "sales/sales", pageNumber);
        }

        [MethodDescription("Sales", "Search for quotes using multiple filters.")]
        public async Task<SimplicateDataCollectionResponse<Quote>?>? SearchQuotes(
            [ParameterDescription("Quote number.")] string? quoteNumber = null,
            [ParameterDescription("Status label.")] string? statusLabel = null,
            [ParameterDescription("Quote subject.")] string? quoteSubject = null,
            [ParameterDescription("Customer reference.")] string? customerReference = null,
            [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(quoteNumber)) filters["[quote_number]"] = $"*{quoteNumber}*";
            if (!string.IsNullOrEmpty(statusLabel)) filters["[quotestatus.label]"] = $"*{statusLabel}*";
            if (!string.IsNullOrEmpty(quoteSubject)) filters["[quote_subject]"] = $"*{quoteSubject}*";
            if (!string.IsNullOrEmpty(customerReference)) filters["[customer_reference]"] = $"*{customerReference}*";

            return await FetchSimplicateDataCollection<Quote>(filters, "sales/quote", pageNumber);
        }

        [MethodDescription("Sales", "Fetches all revenue groups.")]
        public async Task<SimplicateResponseBase<IEnumerable<RevenueGroup>?>?> GetAllRevenueGroups()
        {
            var response = await _httpClient.GetAsync("sales/revenuegroup");

            return await response.FromJson<SimplicateResponseBase<IEnumerable<RevenueGroup>?>>();
        }
    }

}