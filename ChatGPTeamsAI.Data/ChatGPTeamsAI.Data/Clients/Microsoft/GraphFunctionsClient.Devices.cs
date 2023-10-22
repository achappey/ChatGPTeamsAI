using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {
        [MethodDescription("Devices", "Gets devices for the specified criteria.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchDevices(
               [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            var graphClient = GetAuthenticatedClient();

            var filterOptions = new List<QueryOption>();

            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var devices = await graphClient.Devices
                .Request(filterOptions)
                .Top(PAGESIZE)
                .GetAsync();

            var items = devices.CurrentPage.Select(_mapper.Map<Models.Microsoft.Device>);

            return ToChatGPTeamsAIResponse(items, devices.NextPageRequest?.QueryOptions.GetSkipToken());
        }

        [MethodDescription("Devices", "Gets managed devices for the specified criteria.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchManagedDevices(
                    [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            var graphClient = GetAuthenticatedClient();

            var filterOptions = new List<QueryOption>();
            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var devices = await graphClient.DeviceManagement.ManagedDevices
                .Request(filterOptions)
                .Top(PAGESIZE)
                .GetAsync();

            var items = devices.CurrentPage.Select(_mapper.Map<Models.Microsoft.ManagedDevice>);

            return ToChatGPTeamsAIResponse(items, devices.NextPageRequest?.QueryOptions.GetSkipToken());
        }

    }
}