using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Models;
using System.Net.Http.Json;
using ChatGPTeamsAI.Data.Models.Government.NL;

namespace ChatGPTeamsAI.Data.Clients.Government.NL
{
    internal partial class GovernmentNLFunctionsClient
    {
        [MethodDescription("School holidays", "Gets all NL school holidays")]
        public async Task<ChatGPTeamsAIClientResponse?> GetNLSchoolHolidays()
        {

            var items = await _httpClient.GetAsync("schoolholidays?output=json");
            var data = await items.Content.ReadFromJsonAsync<IEnumerable<SchoolHoliday>>();

            return ToChatGPTeamsAIResponse(data?.OrderByDescending(y => y.SchoolYear));
        }

        [MethodDescription("School holidays", "Gets NL school holidays by school year", "ExportNLSchoolHolidaysBySchoolYear")]
        public async Task<ChatGPTeamsAIClientResponse?> GetNLSchoolHolidaysBySchoolYear(
                 [ParameterDescription("School holiday years. For example: 2023-2024 for the school year 2023-2024")] string schoolYear)
        {

            var items = await _httpClient.GetAsync($"schoolholidays/schoolyear/{schoolYear}?output=json");
            var data = await items.Content.ReadFromJsonAsync<SchoolHoliday>();
            var result = data?.Content?.SelectMany(h => h.Vacations!).SelectMany(u => u.Regions!.Select(i =>
                     new VacationRegionData()
                     {
                         EndDate = i.EndDate,
                         StartDate = i.StartDate,
                         Type = u.Type,
                         Region = i.Region
                     }
            ));

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Gets NL school holidays by school year")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportNLSchoolHolidaysBySchoolYear(
                 [ParameterDescription("Year of the school holiday. For example: 2023-2024 for the 2023-2024 school year")] string schoolYear)
        {
            var items = await _httpClient.GetAsync($"schoolholidays/schoolyear/{schoolYear}?output=json");
            var data = await items.Content.ReadFromJsonAsync<SchoolHoliday>();
            var result = data?.Content?.SelectMany(h => h.Vacations!).SelectMany(u => u.Regions!.Select(i =>
                     new VacationRegionData()
                     {
                         EndDate = i.EndDate,
                         StartDate = i.StartDate,
                         Type = u.Type,
                         Region = i.Region
                     }
            ));

            return ToChatGPTeamsAIResponse(result);
        }
    }
}
