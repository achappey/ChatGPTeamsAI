using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {
        [MethodDescription("SharePoint", "Searches for sites based on keywords.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchSites([ParameterDescription("The query to search on.")] string? query = null,
                                      [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            var graphClient = GetAuthenticatedClient();

            var filterOptions = new List<QueryOption>();

            if (!string.IsNullOrEmpty(query))
            {
                filterOptions.Add(new QueryOption("search", query));
            }

            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var sites = await graphClient.Sites
                .Request(filterOptions)
                .Top(PAGESIZE)
                .Header("ConsistencyLevel", "eventual")
                .GetAsync();

            var items = sites.CurrentPage.Select(_mapper.Map<Models.Microsoft.Site>);

            return ToChatGPTeamsAIResponse(items, sites.NextPageRequest?.QueryOptions.GetSkipToken());
        }
    }
}