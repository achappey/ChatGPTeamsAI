using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {

        [MethodDescription("Security", "Gets Microsoft Secure Scores")]
        public async Task<ChatGPTeamsAIClientResponse?> GetSecureScores(
            [ParameterDescription("The number of items to skip")] string? skip = null)
        {
            var filterOptions = new List<QueryOption>();

            if (!string.IsNullOrEmpty(skip))
            {
                filterOptions.Add(new QueryOption("$skip", skip));
            }

            var messages = await _graphClient.Security.SecureScores
                .Request(filterOptions)
                .Top(PAGESIZE)
                .GetAsync();

            return ToChatGPTeamsAIResponse(messages.Select(_mapper.Map<Models.Microsoft.SecureScore>), null, messages.NextPageRequest?.QueryOptions.GetSkip());
        }

    }
}