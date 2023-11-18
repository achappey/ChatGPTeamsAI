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
              [ParameterDescription("Boolean to filter on if the device is managed")] bool? isManaged = null,
              [ParameterDescription("Last sign-in time date at or before, in ISO 8601 format")] string? approximateLastSignInDateTimeBefore = null,
              [ParameterDescription("Last sign-in time date at or after, in ISO 8601 format")] string? approximateLastSignInDateTimeAfter = null,
              [ParameterDescription("String to filter on the display name of the device")] string? displayName = null,
              [ParameterDescription("String to filter on the operating system of the device")] string? operatingSystem = null,
              [ParameterDescription("String to filter on the operating system version of the device")] string? operatingSystemVersion = null,
              [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            var filterOptions = new List<QueryOption>();

            if (isManaged.HasValue)
            {
                filterOptions.Add(new QueryOption("$filter", $"isManaged eq {isManaged.Value}"));
            }

            if (!string.IsNullOrEmpty(approximateLastSignInDateTimeBefore) && DateTime.TryParse(approximateLastSignInDateTimeBefore, out DateTime parsedFromDate))
            {
                filterOptions.Add(new QueryOption("$filter", $"approximateLastSignInDateTime lt '{parsedFromDate:s}Z'"));
            }

            if (!string.IsNullOrEmpty(approximateLastSignInDateTimeAfter) && DateTime.TryParse(approximateLastSignInDateTimeAfter, out DateTime parsedToDate))
            {
                filterOptions.Add(new QueryOption("$filter", $"approximateLastSignInDateTime ge '{parsedToDate:s}Z'"));
            }

            if (!string.IsNullOrEmpty(displayName))
            {
                filterOptions.Add(new QueryOption("$filter", $"startsWith(displayName, '{displayName}')"));
            }

            if (!string.IsNullOrEmpty(operatingSystem))
            {
                filterOptions.Add(new QueryOption("$filter", $"startsWith(operatingSystem, '{operatingSystem}')"));
            }

            if (!string.IsNullOrEmpty(operatingSystemVersion))
            {
                filterOptions.Add(new QueryOption("$filter", $"startsWith(operatingSystemVersion, '{operatingSystemVersion}')"));
            }

            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var devices = await _graphClient.Devices
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
            var filterOptions = new List<QueryOption>();
            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var devices = await _graphClient.DeviceManagement.ManagedDevices
                .Request(filterOptions)
                .Top(PAGESIZE)
                .GetAsync();

            var items = devices.CurrentPage.Select(_mapper.Map<Models.Microsoft.ManagedDevice>);

            return ToChatGPTeamsAIResponse(items, devices.NextPageRequest?.QueryOptions.GetSkipToken());
        }

    }
}