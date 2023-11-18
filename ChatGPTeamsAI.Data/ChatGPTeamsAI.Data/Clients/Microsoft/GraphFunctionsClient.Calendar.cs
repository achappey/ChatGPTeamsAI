using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {
        private async Task<ChatGPTeamsAIClientResponse?> RetrieveEvents(string userId, string? subject, string? organizer, string? date, string? skipToken, int pageSize, bool getAllItems)
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

            var allEvents = new List<Models.Microsoft.Event>();
            IUserEventsCollectionPage events;

            do
            {
                events = await _graphClient.Users[userId].Events
                    .Request(new List<QueryOption>
                    {
                new QueryOption("$filter", string.Join(" and ", filterQueries)),
                new QueryOption("$skiptoken", skipToken)
                    })
                    .Top(pageSize)
                    .GetAsync();

                allEvents.AddRange(events.CurrentPage.Select(_mapper.Map<Models.Microsoft.Event>));

                if (!getAllItems) break; 

                skipToken = events.NextPageRequest?.QueryOptions.GetSkipToken();
            } while (getAllItems && skipToken != null);

            return ToChatGPTeamsAIResponse(allEvents, getAllItems ? null : events.NextPageRequest?.QueryOptions.GetSkipToken());
        }

        [MethodDescription("Export", "Export a list of Outlook calender events for the specified user")]
        public Task<ChatGPTeamsAIClientResponse?> ExportEvents(
            string userId,
            string? subject = null,
            string? organizer = null,
            string? date = null)
        {
            return RetrieveEvents(userId, subject, organizer, date, null, 999, true);
        }

        [MethodDescription("Calendar", "Gets Outlook calender events for the specified user", "ExportEvents")]
        public Task<ChatGPTeamsAIClientResponse?> SearchEvents(
                [ParameterDescription("The user id")] string userId,
                [ParameterDescription("Subject of the event to search for")] string? subject = null,
                [ParameterDescription("Organizer of the event to search for")] string? organizer = null,
                [ParameterDescription("Date in ISO 8601 format")] string? date = null,
                [ParameterDescription("The next page skip token")] string? skipToken = null)
        {
            return RetrieveEvents(userId, subject, organizer, date, skipToken, PAGESIZE, false);
        }

    }
}