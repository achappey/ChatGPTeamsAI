using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {

        [MethodDescription("Projects", "Search for projects using multiple filters.")]
        public async Task<SimplicateDataCollectionResponse<Project>?>? SearchProjects(
            [ParameterDescription("The project name.")] string? projectName = null,
            [ParameterDescription("The project manager's name.")] string? projectManager = null,
            [ParameterDescription("Project status label.")] string? projectStatusLabel = null,
            [ParameterDescription("Organization name.")] string? organizationName = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("Project number.")] string? projectNumber = null,
            [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(projectName)) filters["[name]"] = $"*{projectName}*";
            if (!string.IsNullOrEmpty(projectManager)) filters["[project_manager.name]"] = $"*{projectManager}*";
            if (!string.IsNullOrEmpty(projectStatusLabel)) filters["[project_status.label]"] = $"*{projectStatusLabel}*";
            if (!string.IsNullOrEmpty(organizationName)) filters["[organization.name]"] = $"*{organizationName}*";
            if (!string.IsNullOrEmpty(projectNumber)) filters["[project_number]"] = $"*{projectNumber}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;

            return await FetchSimplicateDataCollection<Project>(filters, "projects/project", pageNumber);
        }

        [MethodDescription("Projects", "Gets a single project by id.")]
        public async Task<SimplicateResponseBase<Project>?> GetProject(
            [ParameterDescription("The project id.")] string projectId)
        {
            return await FetchSimplicateDataItem<Project>($"projects/project/{projectId}");
        }
    }
}