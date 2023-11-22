using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;

namespace ChatGPTeamsAI.Data.Clients.BAG
{
    internal partial class BAGFunctionsClient
    {
        [MethodDescription("Addressen Uitgebreid", "Gets detailed BAG address information by postal code and house number", "ExportBAGAddressesDetailedByPostalCode")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchBAGAddressesDetailedByPostalCode(
                 [ParameterDescription("Postal code. For example: 3452KA or 3448BG")] string postalCode,
                 [ParameterDescription("House number")] string houseNumber,
                 [ParameterDescription("Exact matches only")] bool exactMatch = false,
                 [ParameterDescription("The page number")] long pageNumber = 1)
        {
            var result = await FetchBagPagedResponse(new Dictionary<string, string>() {
                {"postcode", postalCode},
                {"exacteMatch", exactMatch.ToString().ToLowerInvariant()},
                {"huisnummer", houseNumber}},
                 "adressenuitgebreid", pageNumber);

            if (result?.Embedded?.Adressen != null)
            {
                result.Embedded.Adressen = result.Embedded.Adressen?.WithLatLong().ToList();
            }

            return ToChatGPTeamsAIResponse(result?.Embedded?.Adressen, result?.Links);
        }

        [MethodDescription("Export", "Exports detailed BAG address information by postal code and house number")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportBAGAddressesDetailedByPostalCode(
                [ParameterDescription("Postal code. For example: 3452KA or 3448BG")] string postalCode,
                [ParameterDescription("House number")] string houseNumber,
                [ParameterDescription("Exact matches only")] bool exactMatch = false)
        {
            var result = await FetchBagAdresExportResponse(new Dictionary<string, string>() {
                {"postcode", postalCode},
                {"exacteMatch", exactMatch.ToString().ToLowerInvariant()},
                {"huisnummer", houseNumber}},
                 "adressenuitgebreid");

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Addressen Uitgebreid", "Gets detailed BAG address information by query", "ExportBAGAddressesDetailedByQuery")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchBAGAddressesDetailedByQuery(
                [ParameterDescription("The search query")] string query,
                [ParameterDescription("The page number")] long pageNumber = 1)
        {
            var result = await FetchBagPagedResponse(new Dictionary<string, string>() { { "q", query } },
                 "adressenuitgebreid", pageNumber);

            if (result?.Embedded?.Adressen != null)
            {
                result.Embedded.Adressen = result.Embedded.Adressen?.WithLatLong().ToList();
            }

            return ToChatGPTeamsAIResponse(result?.Embedded?.Adressen, result?.Links);
        }

        [MethodDescription("Export", "Exports detailed BAG address information by query")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportBAGAddressesDetailedByQuery(
                  [ParameterDescription("The search query")] string query)
        {
            var result = await FetchBagAdresExportResponse(new Dictionary<string, string>() { { "q", query } }, "adressenuitgebreid");

            return ToChatGPTeamsAIResponse(result);
        }


    }
}
