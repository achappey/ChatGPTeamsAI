using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {

        [MethodDescription("Security", "Gets Microsoft Secure Scores", "ExportSecureScores")]
        public async Task<ChatGPTeamsAIClientResponse?> GetSecureScores(
            [ParameterDescription("The number of items to skip")] string? skip = null)
        {
            var filterOptions = new List<QueryOption>();

            if (!string.IsNullOrEmpty(skip))
            {
                filterOptions.Add(new QueryOption("$skip", skip));
            }

            var items = await _graphClient.Security.SecureScores
                .Request(filterOptions)
                .Top(PAGESIZE)
                .GetAsync();

            return ToChatGPTeamsAIResponse(items.Select(_mapper.Map<Models.Microsoft.SecureScore>), null, items.NextPageRequest?.QueryOptions.GetSkip());
        }
        
        [MethodDescription("Export", "Exports Microsoft Secure Scores")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportSecureScores()
        {
            var filterOptions = new List<QueryOption>();

            var items = await _graphClient.Security.SecureScores
                .Request(filterOptions)
                .Top(999)
                .GetAsync();

            return ToChatGPTeamsAIResponse(items.Select(_mapper.Map<Models.Microsoft.SecureScore>), null, null);
        }

        [MethodDescription("Security", "Gets Microsoft security alerts")]
        public async Task<ChatGPTeamsAIClientResponse?> GetSecurityAlerts(
            [ParameterDescription("The number of items to skip")] string? skip = null)
        {
            var filterOptions = new List<QueryOption>();

            if (!string.IsNullOrEmpty(skip))
            {
                filterOptions.Add(new QueryOption("$skip", skip));
            }

            var items = await _graphClient.Security.Alerts_v2
                .Request(filterOptions)
                .Top(PAGESIZE)
                .GetAsync();

            return ToChatGPTeamsAIResponse(items.Select(_mapper.Map<Models.Microsoft.SecureScore>), null, items.NextPageRequest?.QueryOptions.GetSkip());
        }

    }
}