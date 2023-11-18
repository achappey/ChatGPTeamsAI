using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {

        [MethodDescription("Hours", "Fetches the approval status of each day for each employee", "ExportHoursApprovals")]
        public async Task<ChatGPTeamsAIClientResponse?>? GetHoursApprovals(
                [ParameterDescription("Employee name")] string? employeeName = null,
                [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateAfter = null,
                [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateBefore = null,
                [ParameterDescription("The page number")] long pageNumber = 1)
        {
            dateAfter?.EnsureValidDateFormat();
            dateBefore?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(employeeName)) filters["[employee.name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(dateAfter)) filters["[date][ge]"] = dateAfter;
            if (!string.IsNullOrEmpty(dateBefore)) filters["[date][le]"] = dateBefore;

            var result = await FetchSimplicateDataCollection<HourApproval>(filters, "hours/approval", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of approval status of each day for each employee")]
        public async Task<ChatGPTeamsAIClientResponse?>? ExportHoursApprovals(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Date at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateAfter = null,
            [ParameterDescription("Date at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? dateBefore = null)
        {
            dateAfter?.EnsureValidDateFormat();
            dateBefore?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(employeeName)) filters["[employee.name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(dateAfter)) filters["[date][ge]"] = dateAfter;
            if (!string.IsNullOrEmpty(dateBefore)) filters["[date][le]"] = dateBefore;

            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<HourApproval>($"hours/approval?{queryString}");

            var result = new SimplicateDataCollectionResponse<HourApproval>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }


        [MethodDescription("Hours", "Search for hours", "ExportHours")]
        public async Task<ChatGPTeamsAIClientResponse?>? SearchHours(
                [ParameterDescription("Employee name")] string? employeeName = null,
                [ParameterDescription("Project name")] string? projectName = null,
                [ParameterDescription("Startdate at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? startDateAfter = null,
                [ParameterDescription("Startdate at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? startDateBefore = null,
                [ParameterDescription("The page number")] long pageNumber = 1)
        {
            startDateAfter?.EnsureValidDateFormat();
            startDateBefore?.EnsureValidDateFormat();

            var filters = CreateHoursFilters(employeeName, projectName, startDateAfter, startDateBefore);

            var result = await FetchSimplicateDataCollection<Hour>(filters, "hours/hours", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of hours data")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportHours(
            [ParameterDescription("Employee name")] string? employeeName = null,
            [ParameterDescription("Project name")] string? projectName = null,
            [ParameterDescription("Startdate at or after")] string? startDateAfter = null,
            [ParameterDescription("Startdate at or before")] string? startDateBefore = null)
        {
            startDateAfter?.EnsureValidDateFormat();
            startDateBefore?.EnsureValidDateFormat();

            var filters = CreateHoursFilters(employeeName, projectName, startDateAfter, startDateBefore);
            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<Hour>($"hours/hours?{queryString}");

            var result = new SimplicateDataCollectionResponse<Hour>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreateHoursFilters(
            string? employeeName,
            string? projectName,
            string? startDateAfter,
            string? startDateBefore)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[employee.name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(projectName)) filters["[project.name]"] = $"*{projectName}*";
            if (!string.IsNullOrEmpty(startDateAfter)) filters["[start_date][ge]"] = startDateAfter;
            if (!string.IsNullOrEmpty(startDateBefore)) filters["[start_date][le]"] = startDateBefore;

            return filters;
        }

        [MethodDescription("Hours", "Add a new hours registration")]
        public async Task<ChatGPTeamsAIClientResponse?> AddNewHour(
            [ParameterDescription("Employee ID.")] string employeeId,
            [ParameterDescription("Project ID.")] string projectId,
            [ParameterDescription("Project Service ID.")] string projectServiceId,
            [ParameterDescription("Start Date.")] string startDate,
            [ParameterDescription("End Date.")] string endDate,
            [ParameterDescription("The number of hours.")] double hours,
            [ParameterDescription("Note.")] string? note = null)
        {
            startDate?.EnsureValidDateFormat();
            endDate?.EnsureValidDateFormat();

            var body = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(employeeId)) body["employee_id"] = employeeId;
            if (!string.IsNullOrEmpty(projectId)) body["project_id"] = projectId;
            if (!string.IsNullOrEmpty(projectServiceId)) body["projectservice_id"] = projectServiceId;
            if (startDate != null) body["start_date"] = startDate;
            if (endDate != null) body["end_date"] = endDate;
            if (!string.IsNullOrEmpty(note)) body["note"] = note;
            body["hours"] = hours;

            var response = await _httpClient.PostAsync("hours/hours", body.PrepareJsonContent());

            if (response.IsSuccessStatusCode)
            {
                return ToChatGPTeamsAIResponse(new SimplicateResponseBase<NoOutputResponse>() { Data = SuccessResponse() });
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("Hours", "Fetches all hours types")]
        public async Task<ChatGPTeamsAIClientResponse?> GetAllHoursTypes()
        {
            var response = await _httpClient.GetAsync("hours/hourstype");

            var result = await response.FromJson<SimplicateDataCollectionResponse<HoursType>?>();

            return ToChatGPTeamsAIResponse(result);

        }
    }
}