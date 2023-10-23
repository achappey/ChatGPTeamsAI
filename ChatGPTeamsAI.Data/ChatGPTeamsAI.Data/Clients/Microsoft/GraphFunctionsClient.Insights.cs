using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Microsoft;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {

        [MethodDescription("Me", "Gets trending documents for the current user.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetMyTrendingDocuments(
            [ParameterDescription("The type of the resource.")] ResourceType? resourceType = null)
        {
            var filterOptions = new List<QueryOption>();
            if (resourceType.HasValue)
            {
                filterOptions.Add(new QueryOption("$filter", $"ResourceVisualization/Type eq '{resourceType.Value}'"));
            }

            var insights = await _graphClient.Me.Insights.Trending
                                .Request(filterOptions)
                                .GetAsync();

            var items = insights.CurrentPage.Select(_mapper.Map<Models.Microsoft.Trending>);

            return ToChatGPTeamsAIResponse(items);
        }

        [MethodDescription("Me", "Gets used documents for the current user.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetMyUsedDocuments(
            [ParameterDescription("The type of the resource.")] ResourceType? resourceType = null)
        {
            var trendingRequest = _graphClient.Me.Insights.Used.Request().Top(PAGESIZE);

            if (resourceType.HasValue)
            {
                trendingRequest = trendingRequest.Filter($"ResourceVisualization/Type eq '{resourceType.Value}'");
            }

            var insights = await trendingRequest.GetAsync();
            var items = insights.CurrentPage.Select(_mapper.Map<Models.Microsoft.UsedInsight>);

            return ToChatGPTeamsAIResponse(items);
        }

        [MethodDescription("Me", "Gets documents shared with the current user.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetDocumentsSharedWithMe(
            [ParameterDescription("The type of the resource.")] ResourceType? resourceType = null)
        {
            var sharedRequest = _graphClient.Me.Insights.Shared.Request().Top(PAGESIZE);

            if (resourceType.HasValue)
            {
                sharedRequest = sharedRequest.Filter($"ResourceVisualization/Type eq '{resourceType.Value}'");
            }

            var sharedItems = await sharedRequest.GetAsync();
            var items = sharedItems.CurrentPage.Select(_mapper.Map<Models.Microsoft.SharedInsight>);

            return ToChatGPTeamsAIResponse(items);
        }

    }
}