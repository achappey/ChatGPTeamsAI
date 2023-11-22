using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Output;
using ChatGPTeamsAI.Data.Translations;
using ChatGPTeamsAI.Data.Models.BAG;
using System.Collections.Specialized;
using System.Web;
using System.Net.Http.Json;

namespace ChatGPTeamsAI.Data.Clients.BAG
{
    internal partial class BAGFunctionsClient : BaseClient
    {
        public const string BAG = "Basisregistratie Adressen en Gebouwen";

        private readonly HttpClient _httpClient;

        private const int PAGESIZE = 10;

        internal BAGFunctionsClient(string? apiKey, HttpClient? client = null, ITranslationService? translationService = null) : base(translationService)
        {
            _httpClient = client ?? new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://api.bag.kadaster.nl/lvbag/individuelebevragingen/v2/");

            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept-Crs", "epsg:28992");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ChatGPTeamsAI");
        }

        public override async Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Models.Input.Action action)
        {
            var functionDefintition = GetAvailableActions().FirstOrDefault(a => a.Name == action.Name) ?? throw new ArgumentException("Action missing");
            var result = await this.ExecuteMethodAsync(action) as ChatGPTeamsAIClientResponse ?? throw new ArgumentException("Something went wrong");
            var metadata = result.Properties != null && result.Properties.ContainsKey("pagingLinks") ? result.Properties["pagingLinks"] as PagingLinks : null;

            result.NextPageAction = GetPageAction(action, functionDefintition, metadata, true);
            result.PreviousPageAction = GetPageAction(action, functionDefintition, metadata, false);
            result.ExportPageAction = GetExportAction(action, functionDefintition);

            result.ExecutedAction = action;
            return result;
        }

        private static Models.Input.Action? GetExportAction(Models.Input.Action currentPageAction,
                   ActionDescription action,
                   string pageNumberPropertyName = "pageNumber")
        {
            if (string.IsNullOrEmpty(action.ExportAction))
            {
                return null;
            }

            var pageActionEntities = new Dictionary<string, object?>(
                         currentPageAction.Entities ?? new Dictionary<string, object?>());

            if (pageActionEntities.ContainsKey(pageNumberPropertyName))
            {
                pageActionEntities = pageActionEntities.Where(a => a.Key != pageNumberPropertyName).ToDictionary(a => a.Key, a => a.Value);
            }

            return new Models.Input.Action
            {
                Name = action.ExportAction,
                Entities = pageActionEntities
            };
        }

        private static Models.Input.Action? GetPageAction(Models.Input.Action currentPageAction,
            ActionDescription action, PagingLinks? pagingLinks, bool isNextPage,
            string pageNumberPropertyName = "pageNumber")
        {
            var hasPageNumberParameter = action.Parameters?.Properties?.Any(p => p.Name == pageNumberPropertyName) ?? false;
            if (!hasPageNumberParameter || pagingLinks == null)
            {
                return null;
            }

            var hasNextPage = pagingLinks.NextPage;
            var hasPrevPage = pagingLinks.CurrentPage > 1;

            if ((isNextPage && !hasNextPage.HasValue) || (!isNextPage && !hasPrevPage))
            {
                return null;
            }

            var pageActionEntities = new Dictionary<string, object?>(
                currentPageAction.Entities ?? new Dictionary<string, object?>());

            var newPageNumber = isNextPage ? pagingLinks.NextPage : pagingLinks.CurrentPage - 1;

            pageActionEntities[pageNumberPropertyName] = newPageNumber;

            return new Models.Input.Action
            {
                Name = currentPageAction.Name,
                Entities = pageActionEntities
            };
        }

        public override IEnumerable<ActionDescription> GetAvailableActions()
        {
            return typeof(BAGFunctionsClient).GetTypedFunctionDefinitions(BAG);
        }

        private ChatGPTeamsAIClientResponse? ToChatGPTeamsAIResponse<T>(IEnumerable<T>? response, PagingLinks? pagingLinks = null)
        {
            if (response == null)
            {
                return new ChatGPTeamsAIClientResponse()
                {
                    Type = typeof(T).ToString(),
                    Error = "Something went wrong"
                };
            }

            var dataCard = RenderCard(response);

            Dictionary<string, object>? properties = null;

            if (pagingLinks != null)
            {
                properties = new Dictionary<string, object> { { "pagingLinks", pagingLinks } };
            }

            return new ChatGPTeamsAIClientResponse()
            {
                Data = response?.RenderData(),
                DataCard = dataCard,
                Type = typeof(T).ToString(),
                Properties = properties
            };
        }

        private static NameValueCollection BuildQueryString(Dictionary<string, string>? filters)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (!string.IsNullOrEmpty(filter.Value))
                    {
                        queryString[$"{filter.Key}"] = $"{filter.Value}";
                    }
                }
            }

            return queryString;
        }

        private Task<BAGResponse?> FetchBagPagedResponse(
                  Dictionary<string, string>? filters,
                  string endpointUrl,
                  long page = 1)
        {
            if (page <= 0) page = 1;

            filters = filters != null ? filters : new Dictionary<string, string>();
            filters.Add("pageSize", PAGESIZE.ToString());
            filters.Add("page", page.ToString());

            return FetchBagResponse(filters, endpointUrl);
        }

        private async Task<BAGResponse?> FetchBagResponse(
                Dictionary<string, string>? filters,
                string endpointUrl)
        {
            var queryString = BuildQueryString(filters);

            var response = await _httpClient.GetAsync($"{endpointUrl}?{queryString}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BAGResponse?>();
            }

            throw new Exception(response.ReasonPhrase);
        }

        private async Task<IEnumerable<Address>?> FetchBagAdresExportResponse(
                Dictionary<string, string>? filters,
                string endpointUrl)
        {
            filters = filters != null ? filters : new Dictionary<string, string>();
            filters.Add("pageSize", "100");
            filters.Add("page", "1");

            var items = await FetchBagResponse(filters, endpointUrl);
            List<Address> result = new();

            if (items?.Embedded?.Adressen != null)
            {
                result.AddRange(items.Embedded.Adressen);
            }

            if (items != null && items?.Links != null)
            {
                while (items!.Links!.NextPage.HasValue)
                {
                    filters["page"] = items.Links.NextPage.Value.ToString();

                    await Task.Delay(40);

                    items = await FetchBagResponse(filters, endpointUrl);

                    if (items?.Embedded?.Adressen != null)
                    {
                        result.AddRange(items.Embedded.Adressen);
                    }
                }
            }

            if (result != null)
            {
                result = result.WithLatLong().ToList();
            }

            return result;
        }
    }

}


