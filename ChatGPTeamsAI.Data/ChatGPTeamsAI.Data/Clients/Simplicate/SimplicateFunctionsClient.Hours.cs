using System.Web;
using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {
        [MethodDescription("Hours", "Search for hours using multiple filters.")]
        public async Task<ChatGPTeamsAIClientResponse?>? SearchHours(
                [ParameterDescription("Employee name.")] string? employeeName = null,
                [ParameterDescription("Project name.")] string? projectName = null,
                [ParameterDescription("Startdate at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? startDateAfter = null,
                [ParameterDescription("Startdate at or before this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? startDateBefore = null,
                [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            startDateAfter?.EnsureValidDateFormat();
            startDateBefore?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(employeeName)) filters["[employee.name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(projectName)) filters["[project.name]"] = $"*{projectName}*";
            if (!string.IsNullOrEmpty(startDateAfter)) filters["[start_date][ge]"] = startDateAfter;
            if (!string.IsNullOrEmpty(startDateBefore)) filters["[start_date][le]"] = startDateBefore;

            var result = await FetchSimplicateDataCollection<Hour>(filters, "hours/hours", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Hours", "Gets total hours per employee using multiple filters.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetTotalHoursPerEmployee(
              [ParameterDescription("Employee name.")] string? employeeName = null,
              [ParameterDescription("Project name.")] string? projectName = null,
              [ParameterDescription("Status.")] HourStatus? status = null,
              [ParameterDescription("Startdate at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? startDateAfter = null,
              [ParameterDescription("Startdate at or before this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? startDateBefore = null)
        {
            startDateAfter?.EnsureValidDateFormat();
            startDateBefore?.EnsureValidDateFormat();

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(employeeName)) queryString["q[employee.name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(projectName)) queryString["q[project.name]"] = $"*{projectName}*";
            if (!string.IsNullOrEmpty(startDateAfter)) queryString["q[start_date][ge]"] = startDateAfter;
            if (!string.IsNullOrEmpty(startDateBefore)) queryString["q[start_date][le]"] = startDateBefore;

            var response = await _httpClient.PagedRequest<Hour>($"hours/hours?{queryString}");

            var groupedHours = response
                .Where(a => !status.HasValue || (status.HasValue && a.Status == status.Value.GetEnumMemberAttributeValue()))
                .GroupBy(h => new { h.Employee?.Id, h.Employee?.Name, h.Status })
                .Select(g => new
                {
                    EmployeeName = g.Key.Name,
                    Status = status.HasValue ? StringExtensions.GetEnumMemberAttributeValue(status.Value) : g.Key.Status,
                    TotalHours = g.Sum(h => h.Hours)
                })
                .Where(y => y.TotalHours > 0);

            return ToChatGPTeamsAIResponse(new SimplicateDataCollectionResponse<dynamic>() { Data = groupedHours });
        }

        [MethodDescription("Hours", "Gets hours per project using multiple filters.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetHoursPerProject(
              [ParameterDescription("Employee name.")] string? employeeName = null,
              [ParameterDescription("Project name.")] string? projectName = null,
              [ParameterDescription("Status.")] HourStatus? status = null,
              [ParameterDescription("Startdate at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? startDateAfter = null,
              [ParameterDescription("Startdate at or before this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? startDateBefore = null)
        {
            startDateAfter?.EnsureValidDateFormat();
            startDateBefore?.EnsureValidDateFormat();

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(employeeName)) queryString["q[employee.name]"] = $"*{employeeName}*";
            if (!string.IsNullOrEmpty(projectName)) queryString["q[project.name]"] = $"*{projectName}*";
            if (!string.IsNullOrEmpty(startDateAfter)) queryString["q[start_date][ge]"] = startDateAfter;
            if (!string.IsNullOrEmpty(startDateBefore)) queryString["q[start_date][le]"] = startDateBefore;

            var response = await _httpClient.PagedRequest<Hour>($"hours/hours?{queryString}");

            var groupedHours = response
                .Where(a => !status.HasValue || (status.HasValue && a.Status == status.Value.GetEnumMemberAttributeValue()))
                .GroupBy(h => new { h.Project?.Id, h.Project?.Name, h.Status })
                .Select(g => new
                {
                    ProjectName = g.Key.Name,
                    Status = status.HasValue ? StringExtensions.GetEnumMemberAttributeValue(status.Value) : g.First().Status,
                    TotalHours = g.Sum(h => h.Hours)
                })
                .Where(y => y.TotalHours > 0);

               return ToChatGPTeamsAIResponse(new SimplicateDataCollectionResponse<dynamic>() { Data = groupedHours });
        }

        [MethodDescription("Hours", "Add a new hours registration.")]
        public async Task<NoOutputResponse> AddNewHour(
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
                return SuccessResponse();
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("Hours", "Fetches all hours types.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetAllHoursTypes()
        {
            var response = await _httpClient.GetAsync("hours/hourstype");

            var result = await response.FromJson<SimplicateDataCollectionResponse<HoursType>?>();

            return ToChatGPTeamsAIResponse(result);

        }
    }
}