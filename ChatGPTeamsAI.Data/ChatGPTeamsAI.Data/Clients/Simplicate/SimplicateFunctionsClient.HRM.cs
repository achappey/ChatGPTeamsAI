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

            var filters = CreateEmployeeFilters(employeeName, function, employmentStatus, createdAfter);
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

            var filters = CreateEmployeeFilters(employeeName, function, employmentStatus, createdAfter);
            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<Employee>($"hrm/employee?{queryString}");

            var result = new SimplicateDataCollectionResponse<Employee>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }


        private Dictionary<string, string> CreateEmployeeFilters(
            string? employeeName,
            string? function,
            string? employmentStatus,
            string? createdAfter)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(function)) filters["[function]"] = $"*{function}*";
            if (!string.IsNullOrEmpty(employmentStatus)) filters["[employment_status]"] = $"*{employmentStatus}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;

            return filters;
        }


        [MethodDescription("HRM", "Search for employee leave balances", "ExportLeaveBalances")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchLeaveBalances(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Year")] long? year = null,
            [ParameterDescription("Pagenumber")] long pageNumber = 1)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[employee.name]"] = $"*{employeeName}*"; ;
            if (year.HasValue) filters["[year]"] = year.Value.ToString();

            var result = await FetchSimplicateDataCollection<LeaveBalance>(filters, "hrm/leavebalance", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of employee leave balances")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportLeaveBalances(
          [ParameterDescription("Employee name")] string? employeeName = null,
          [ParameterDescription("Year")] long? year = null)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(employeeName)) queryString["q[name]"] = $"*{employeeName}*";
            if (year.HasValue) queryString["q[year]"] = year.Value.ToString();

            var response = await _httpClient.PagedRequest<LeaveBalance>($"hrm/leavebalance?{queryString}");

            var result = new SimplicateDataCollectionResponse<LeaveBalance>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("HRM", "Search for employee absences", "ExportAbsences")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchAbsences(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Year")] long? year = null,
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

        [MethodDescription("Export", "Exports a list of employee absences")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportAbsences(
                [ParameterDescription("Employee name")] string? employeeName = null,
                [ParameterDescription("Year")] long? year = null,
                [ParameterDescription("Absence type name")] string? absenceTypeName = null)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(employeeName)) queryString["q[employee.name]"] = $"*{employeeName}*";
            if (year.HasValue) queryString["q[year]"] = year.Value.ToString();
            if (!string.IsNullOrEmpty(absenceTypeName)) queryString["q[absencetype.label]"] = absenceTypeName;

            var response = await _httpClient.PagedRequest<Absence>($"hrm/absence?{queryString}");

            var result = new SimplicateDataCollectionResponse<Absence>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreateTimeTableFilters(
            string? employeeName = null,
            string? startDateAfter = null,
            string? endDateBefore = null)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[employee.name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(startDateAfter)) filters["[start_date][ge]"] = startDateAfter;
            if (!string.IsNullOrEmpty(endDateBefore)) filters["[end_date][le]"] = endDateBefore;
            return filters;
        }

        [MethodDescription("HRM", "Search for timetables", "ExportTimetables")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchTimetables(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Start date at or after (format: yyyy-MM-dd HH:mm:ss)")] string? startDateAfter = null,
            [ParameterDescription("End date at or before (format: yyyy-MM-dd HH:mm:ss)")] string? endDateBefore = null,
            [ParameterDescription("The page number")] long pageNumber = 1)
        {
            startDateAfter?.EnsureValidDateFormat();
            endDateBefore?.EnsureValidDateFormat();

            var filters = CreateTimeTableFilters(employeeName, startDateAfter, endDateBefore);

            var result = await FetchSimplicateDataCollection<Timetable>(filters, "hrm/timetable", pageNumber);
            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of timetables")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportTimetables(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Start date at or after (format: yyyy-MM-dd HH:mm:ss)")] string? startDateAfter = null,
            [ParameterDescription("End date at or before (format: yyyy-MM-dd HH:mm:ss)")] string? startDateBefore = null,
            [ParameterDescription("The page number")] long pageNumber = 1)
        {
            startDateAfter?.EnsureValidDateFormat();
            startDateBefore?.EnsureValidDateFormat();

            var filters = CreateTimeTableFilters(employeeName, startDateAfter, startDateBefore);

            var result = await FetchSimplicateDataCollection<Timetable>(filters, "hrm/timetable", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("HRM", "Gets all details of a single employee")]
        public async Task<ChatGPTeamsAIClientResponse?> GetEmployee(
            [ParameterDescription("The employee id")] string employeeId)
        {
            var result = await FetchSimplicateDataItem<Employee>($"hrm/employee/{employeeId}");

            return ToChatGPTeamsAIResponse(result);
        }
    }
}