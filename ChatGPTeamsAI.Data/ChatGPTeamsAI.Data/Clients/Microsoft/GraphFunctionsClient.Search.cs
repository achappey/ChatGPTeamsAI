using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {
        [MethodDescription("SharePoint", "Searches content across SharePoint and OneDrive resources.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchDriveContent(
            [ParameterDescription("The search query.")] string query,
            [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            return await SearchContent(query, EntityType.DriveItem, skipToken);
        }

        [MethodDescription("Mail", "Searches Outlook messages.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchOutlookContent(
                    [ParameterDescription("The search query.")] string query,
                    [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            return await SearchContent(query, EntityType.Message, skipToken);
        }

        [MethodDescription("Teams", "Searches chat messages.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchChatContent(
                              [ParameterDescription("The search query.")] string query,
                              [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            return await SearchContent(query, EntityType.ChatMessage, skipToken);
        }

        private async Task<ChatGPTeamsAIClientResponse?> SearchContent(
                              string query, EntityType type,
                              string? skipToken = null)
        {
            

            if (string.IsNullOrEmpty(query))
            {
                query = "*";
            }

            var filterOptions = new List<QueryOption>();
            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var searchRequest = new SearchRequestObject()
            {
                Query = new SearchQuery
                {
                    QueryString = query,
                },
                EntityTypes = new List<EntityType> { type },
                From = string.IsNullOrEmpty(skipToken) ? 0 : int.Parse(skipToken),
                Size = PAGESIZE
            };

            var searchResponse = await _graphClient.Search
                .Query(new List<SearchRequestObject>() {
                         searchRequest
                })
                .Request(filterOptions)
                .PostAsync();

            var items = searchResponse?.FirstOrDefault()?.HitsContainers?.FirstOrDefault()?.Hits.Select(a => _mapper.Map<Models.Microsoft.SearchHit>(a));

            return ToChatGPTeamsAIResponse(items, searchResponse?.NextPageRequest?.QueryOptions.GetSkipToken());
        }

    }
}