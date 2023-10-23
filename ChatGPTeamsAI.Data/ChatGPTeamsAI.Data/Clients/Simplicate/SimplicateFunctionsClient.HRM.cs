﻿using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {
        [MethodDescription("HRM", "Fetches all leave types.")]
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

        [MethodDescription("HRM", "Search for employees using multiple filters.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchEmployees(
            [ParameterDescription("Employee name.")] string? employeeName = null,
            [ParameterDescription("Function.")] string? function = null,
            [ParameterDescription("Employment status (e.g., active).")] string? employmentStatus = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("The page number.")] long pageNumber = 1)
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



    }
}