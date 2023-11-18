using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Models;
using Azure.Maps.Search.Models;
using Azure.Maps.Search;
using Azure.Core.GeoJson;

namespace ChatGPTeamsAI.Data.Clients.Azure.Maps
{
    internal partial class AzureMapsFunctionsClient
    {
        [MethodDescription("Search", "Search addresses and POIs by query and location", "ExportAddresses")]
        public Task<ChatGPTeamsAIClientResponse?> SearchAddresses(
                [ParameterDescription("Search query")] string query,
                [ParameterDescription("Latitude")] double latitude,
                [ParameterDescription("Longitude")] double longitude,
                [ParameterDescription("Language in EITF format (en-US, nl-NL, etc)")] string? language = null,
                [ParameterDescription("The number of items to skip")] long? skip = 0)
        {
            return QueryAddresses(query, latitude, longitude, PAGESIZE, language, skip.HasValue ? (int)skip.Value : 0);
        }

        private async Task<ChatGPTeamsAIClientResponse?> QueryAddresses(
                string query,
                double latitude,
                double longitude,
                int top = PAGESIZE,
                string? language = null,
                long? skip = 0)
        {
            SearchAddressResult searchResult = await _mapsSearchClient.FuzzySearchAsync(
                query, new FuzzySearchOptions
                {
                    Coordinates = new GeoPosition(longitude, latitude),
                    Language = language != null ? language : SearchLanguage.EnglishUsa,
                    Top = top,
                    Skip = skip.HasValue ? (int)skip.Value : 0
                });

            var items = searchResult.Results.Select(_mapper.Map<Models.Azure.Maps.SearchAddress>);

            return ToChatGPTeamsAIResponse(items, skip.HasValue ? skip + PAGESIZE : PAGESIZE);
        }

        [MethodDescription("Export", "Exports a maps search results of addresses")]
        public Task<ChatGPTeamsAIClientResponse?> ExportAddresses(
            [ParameterDescription("Search query")] string query,
            [ParameterDescription("Latitude")] double latitude,
            [ParameterDescription("Longitude")] double longitude,
            [ParameterDescription("Language in EITF format (en-US, nl-NL, etc)")] string? language = null,
            [ParameterDescription("The number of items to skip")] long? skip = 0)
        {
            return QueryAddresses(query, latitude, longitude, 100, language, 0);
        }
    }
}
