using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;

namespace ChatGPTeamsAI.Data.Clients.BAG
{
    internal partial class BAGFunctionsClient
    {
        [MethodDescription("Addressen", "Gets BAG information by pand identification", "ExportBAGAddressesByBuilding")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchBAGAddressesByBuilding(
                 [ParameterDescription("Pand identification")] string pandIdentification,
                 [ParameterDescription("The page number")] long pageNumber = 1)
        {
            var result = await FetchBagPagedResponse(new Dictionary<string, string>() { { "pandIdentificatie", pandIdentification } }, "adressen", pageNumber);

            if (result?.Embedded?.Adressen != null)
            {
                result.Embedded.Adressen = result.Embedded.Adressen?.WithLatLong().ToList();
            }

            return ToChatGPTeamsAIResponse(result?.Embedded?.Adressen, result?.Links);
        }

        [MethodDescription("Export", "Exports BAG information by pand identification")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportBAGAddressesByBuilding(
                 [ParameterDescription("Pand identification")] string pandIdentification)
        {
            var result = await FetchBagAdresExportResponse(new Dictionary<string, string>() { { "pandIdentificatie", pandIdentification } }, "adressen");

            return ToChatGPTeamsAIResponse(result);
        }

    }
}
