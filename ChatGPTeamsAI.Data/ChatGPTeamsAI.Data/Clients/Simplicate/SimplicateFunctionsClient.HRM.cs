using System.Web;
using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {
        [MethodDescription("HRM", "Fetches all leave types")]
        public async Task<ChatGPTeamsAIClientResponse?> GetAllLeaveTypes()
        {
            var response = await _httpClient.GetAsync("hrm/leavetype");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.FromJson<SimplicateDataCollectionResponse<LeaveType>>();

                return ToChatGPTeamsAIResponse(result);
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("HRM", "Search for employees", "ExportEmployees")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchEmployees(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Function")] string? function = null,
            [ParameterDescription("Employment status (e.g., active)")] string? employmentStatus = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
            [ParameterDescription("Pagenumber")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(function)) filters["[function]"] = $"*{function}*";
            if (!string.IsNullOrEmpty(employmentStatus)) filters["[employment_status]"] = $"*{employmentStatus}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;

            var result = await FetchSimplicateDataCollection<Employee>(filters, "hrm/employee", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of employees")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportEmployees(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Function")] string? function = null,
            [ParameterDescription("Employment status (e.g., active)")] string? employmentStatus = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(function)) filters["[function]"] = $"*{function}*";
            if (!string.IsNullOrEmpty(employmentStatus)) filters["[employment_status]"] = $"*{employmentStatus}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(employeeName)) queryString["q[name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(function)) queryString["q[function]"] = $"*{function}*";
            if (!string.IsNullOrEmpty(employmentStatus)) queryString["q[employment_status]"] = $"{employmentStatus}";
            if (!string.IsNullOrEmpty(createdAfter)) queryString["q[created_at][ge]"] = createdAfter;

            var response = await _httpClient.PagedRequest<Employee>($"hrm/employee?{queryString}");

            var result = new SimplicateDataCollectionResponse<Employee>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("HRM", "Search for employee leave balances")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchLeaveBalances(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Year")] int? year = null,
            [ParameterDescription("Pagenumber")] long pageNumber = 1)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[employee.name]"] = $"*{employeeName}*"; ;
            if (year.HasValue) filters["[year]"] = year.Value.ToString();

            var result = await FetchSimplicateDataCollection<LeaveBalance>(filters, "hrm/leavebalance", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("HRM", "Search for employee absences")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchAbsences(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Year")] int? year = null,
            [ParameterDescription("Absence type name")] string? absenceTypeName = null,
            [ParameterDescription("Pagenumber")] long pageNumber = 1)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[employee.name]"] = $"*{employeeName}*";
            if (year.HasValue) filters["[year]"] = year.Value.ToString();
            if (!string.IsNullOrEmpty(absenceTypeName)) filters["[absencetype.label]"] = absenceTypeName;

            var result = await FetchSimplicateDataCollection<Absence>(filters, "hrm/absence", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }



    }
}