using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {

        [MethodDescription("SharePoint", "Retrieves the pages for a specific site.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetSitePages(
            [ParameterDescription("The ID of the site.")] string siteId,
            [ParameterDescription("The name to filter on.")] string? name = null,
            [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            var filterOptions = new List<QueryOption>();
            
            if (!string.IsNullOrEmpty(name))
            {
                filterOptions.Add(new QueryOption("$filter", $"contains(name, '{name}')"));
            }

            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var pages = await _graphClient.Sites[siteId].Pages
                        .Request(filterOptions)
                        .GetAsync();

            var items = pages.CurrentPage.Select(_mapper.Map<Models.Microsoft.Page>);

            return ToChatGPTeamsAIResponse(items, pages.NextPageRequest?.QueryOptions.GetSkipToken());
        }
    }
}