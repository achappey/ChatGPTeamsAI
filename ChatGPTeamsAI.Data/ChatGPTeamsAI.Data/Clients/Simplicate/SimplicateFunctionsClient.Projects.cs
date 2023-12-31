﻿using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {

        [MethodDescription("Projects", "Search for projects", "ExportProjects")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchProjects(
            [ParameterDescription("The project name")] string? projectName = null,
            [ParameterDescription("The project manager's name")] string? projectManager = null,
            [ParameterDescription("Project status label")] string? projectStatusLabel = null,
            [ParameterDescription("Organization name")] string? organizationName = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
            [ParameterDescription("Project number")] string? projectNumber = null,
            [ParameterDescription("The page number")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreateProjectFilters(projectName, projectManager, projectStatusLabel, organizationName, createdAfter, projectNumber);
            var result = await FetchSimplicateDataCollection<Project>(filters, "projects/project", pageNumber, "-project_number");

            return ToChatGPTeamsAIResponse(result);

        }

        [MethodDescription("Export", "Exports a list of projects")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportProjects(
            [ParameterDescription("The project name")] string? projectName = null,
            [ParameterDescription("The project manager's name")] string? projectManager = null,
            [ParameterDescription("Project status label")] string? projectStatusLabel = null,
            [ParameterDescription("Organization name")] string? organizationName = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
            [ParameterDescription("Project number.")] string? projectNumber = null)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreateProjectFilters(projectName, projectManager, projectStatusLabel, organizationName, createdAfter, projectNumber);
            var queryString = BuildQueryString(filters, "-project_number");

            var response = await _httpClient.PagedRequest<Project>($"projects/project?{queryString}");

            var result = new SimplicateDataCollectionResponse<Project>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreateProjectFilters(
            string? projectName,
            string? projectManager,
            string? projectStatusLabel,
            string? organizationName,
            string? createdAfter,
            string? projectNumber)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(projectName)) filters["[name]"] = $"*{projectName}*";
            if (!string.IsNullOrEmpty(projectManager)) filters["[project_manager.name]"] = $"*{projectManager}*";
            if (!string.IsNullOrEmpty(projectStatusLabel)) filters["[project_status.label]"] = $"*{projectStatusLabel}*";
            if (!string.IsNullOrEmpty(organizationName)) filters["[organization.name]"] = $"*{organizationName}*";
            if (!string.IsNullOrEmpty(projectNumber)) filters["[project_number]"] = $"*{projectNumber}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            return filters;
        }


        [MethodDescription("Projects", "Gets all details of a single project")]
        public async Task<ChatGPTeamsAIClientResponse?> GetProject(
            [ParameterDescription("The project id.")] string projectId)
        {
            var result = await FetchSimplicateDataItem<Project>($"projects/project/{projectId}");

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Projects", "Search for project documents")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchProjectDocuments(
                  [ParameterDescription("Type of the document")] string? documentType = null,
                  [ParameterDescription("Title of the document")] string? title = null,
                  [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdAfter = null,
                  [ParameterDescription("Created at or before this date and time (format: yyyy-MM-dd HH:mm:ss)")] string? createdBefore = null,
                  [ParameterDescription("Page number")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();
            createdBefore?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(title)) filters["[title]"] = $"*{title}*";
            if (!string.IsNullOrEmpty(documentType)) filters["[document_type.label]"] = $"*{documentType}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(createdBefore)) filters["[created_at][le]"] = createdBefore;

            var result = await FetchSimplicateDataCollection<ProjectDocument>(filters, "projects/document", pageNumber, "-created_at");

            return ToChatGPTeamsAIResponse(result);
        }
    }
}