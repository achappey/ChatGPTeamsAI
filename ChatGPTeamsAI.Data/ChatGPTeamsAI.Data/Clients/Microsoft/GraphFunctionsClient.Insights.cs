using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Microsoft;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {

        [MethodDescription("Documents", "Gets trending documents for the current user.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetMyTrendingDocuments(
            [ParameterDescription("The type of the resource.")] ResourceType? resourceType = null,
            [ParameterDescription("The number of items to skip")] string? skip = null)
        {
            var filterOptions = new List<QueryOption>();
            if (resourceType.HasValue)
            {
                filterOptions.Add(new QueryOption("$filter", $"ResourceVisualization/Type eq '{resourceType.Value}'"));
            }

            if (!string.IsNullOrEmpty(skip))
            {
                filterOptions.Add(new QueryOption("$skip", skip));
            }

            var insights = await _graphClient.Me.Insights.Trending
                                .Request(filterOptions)
                                .Top(PAGESIZE)
                                .GetAsync();

            var nextSkip = string.IsNullOrEmpty(skip) ? PAGESIZE.ToString() : (int.Parse(skip) + PAGESIZE).ToString();
            var items = insights.CurrentPage.Select(_mapper.Map<Models.Microsoft.Trending>);

            return ToChatGPTeamsAIResponse(items, null, nextSkip);
        }

        [MethodDescription("Documents", "Gets used documents for the current user.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetMyUsedDocuments(
            [ParameterDescription("The type of the resource.")] ResourceType? resourceType = null,
            [ParameterDescription("The number of items to skip")] string? skip = null)
        {

            var filterOptions = new List<QueryOption>();

            if (resourceType.HasValue)
            {
                filterOptions.Add(new QueryOption("$filter", $"ResourceVisualization/Type eq '{resourceType.Value}'"));
            }

            if (!string.IsNullOrEmpty(skip))
            {
                filterOptions.Add(new QueryOption("$skip", skip));
            }

            var insights = await _graphClient.Me.Insights.Used
                            .Request(filterOptions)
                            .Top(PAGESIZE)
                            .GetAsync();

            var nextSkip = string.IsNullOrEmpty(skip) ? PAGESIZE.ToString() : (int.Parse(skip) + PAGESIZE).ToString();
            var items = insights.CurrentPage.Select(_mapper.Map<Models.Microsoft.UsedInsight>);

            return ToChatGPTeamsAIResponse(items, null, nextSkip);
        }

        [MethodDescription("Documents", "Gets documents shared with the current user.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetDocumentsSharedWithMe(
            [ParameterDescription("The type of the resource.")] ResourceType? resourceType = null,
            [ParameterDescription("The number of items to skip")] string? skip = null)
        {

            var filterOptions = new List<QueryOption>();

            if (resourceType.HasValue)
            {
                filterOptions.Add(new QueryOption("$filter", $"ResourceVisualization/Type eq '{resourceType.Value}'"));
            }

            if (!string.IsNullOrEmpty(skip))
            {
                filterOptions.Add(new QueryOption("$skip", skip));
            }

            var insights = await _graphClient.Me.Insights.Shared
                            .Request(filterOptions)
                            .Top(PAGESIZE)
                            .GetAsync();

            var nextSkip = string.IsNullOrEmpty(skip) ? PAGESIZE.ToString() : (int.Parse(skip) + PAGESIZE).ToString();
            var items = insights.CurrentPage.Select(_mapper.Map<Models.Microsoft.SharedInsight>);

            return ToChatGPTeamsAIResponse(items, null, nextSkip);
        }

    }
}