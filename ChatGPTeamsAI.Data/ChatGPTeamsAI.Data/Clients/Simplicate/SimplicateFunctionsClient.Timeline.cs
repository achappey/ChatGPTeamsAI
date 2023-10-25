using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {
        [MethodDescription("Timeline", "Search for Simplicate timeline events")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchTimelineMessages(
            [ParameterDescription("Created at or after this date and time in ISO 8601 format (yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("Created at or before this date and time in ISO 8601 format (yyyy-MM-dd HH:mm:ss).")] string? createdBefore = null,
            [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();
            createdBefore?.EnsureValidDateFormat();

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(createdBefore)) filters["[created_at][le]"] = createdBefore;

            var result = await FetchSimplicateDataCollection<TimelineMessage>(filters, "timeline/message", pageNumber);
            
            return ToChatGPTeamsAIResponse(result);
        }

    }
}
