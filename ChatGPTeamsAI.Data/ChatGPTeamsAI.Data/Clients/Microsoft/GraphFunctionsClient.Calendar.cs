using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {
        [MethodDescription("Calendar", "Gets Outlook calender events for the specified user")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchEvents(
                [ParameterDescription("The user id")] string userId,
                [ParameterDescription("Subject of the event to search for")] string? subject = null,
                [ParameterDescription("Organizer of the event to search for")] string? organizer = null,
                [ParameterDescription("Date in ISO 8601 format")] string? date = null,
                [ParameterDescription("The next page skip token")] string? skipToken = null)
        {
            var filterQueries = new List<string>();

            if (!string.IsNullOrEmpty(subject))
            {
                filterQueries.Add($"contains(subject, '{subject}')");
            }

            if (!string.IsNullOrEmpty(organizer))
            {
                filterQueries.Add($"organizer/emailAddress/address eq '{organizer}'");
            }

            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out DateTime parsedFromDate))
            {
                DateTime parsedToDate = parsedFromDate.AddDays(1);
                filterQueries.Add($"start/dateTime ge '{parsedFromDate:s}Z'");
                filterQueries.Add($"end/dateTime lt '{parsedToDate:s}Z'");
            }

            var filterOptions = new List<QueryOption>();
            
            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var filterQuery = string.Join(" and ", filterQueries);
            var selectQuery = "id,subject,start,end";

            var events = await _graphClient.Users[userId].Events
                .Request(filterOptions)
                .Filter(filterQuery)
                .Top(PAGESIZE)
                .Select(selectQuery)
                .GetAsync();

            var items = events.CurrentPage.Select(_mapper.Map<Models.Microsoft.Event>);

            return ToChatGPTeamsAIResponse(items, events.NextPageRequest?.QueryOptions.GetSkipToken());
        }



    }
}