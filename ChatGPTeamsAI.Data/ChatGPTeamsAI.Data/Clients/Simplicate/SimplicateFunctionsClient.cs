using System.Web;
using ChatGPTeamsAI.Data.Models.Simplicate;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Cards.Simplicate;
using ChatGPTeamsAI.Data.Models.Output;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient : BaseClient
    {
        public const string SIMPLICATE = "Simplicate";

        private readonly HttpClient _httpClient;

        private const int PAGESIZE = 5;

        internal SimplicateFunctionsClient(SimplicateToken token, HttpClient? client = null)
        {
            _httpClient = client ?? new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://{token.Environment}.simplicate.nl/api/v2/");

            _httpClient.DefaultRequestHeaders.Add("Authentication-Key", token.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("Authentication-Secret", token.ApiSecret);

            _cardRenderers.Add(typeof(Project), new ProjectCardRenderer());
            _cardRenderers.Add(typeof(List<Project>), new ProjectsCardRenderer());
        }

        public override async Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Models.Input.Action action)
        {
            var result = await this.ExecuteMethodAsync(action) as ChatGPTeamsAIClientResponse ?? throw new ArgumentException("Something went wrong");
            var functionDefintition = GetAvailableActions().FirstOrDefault(a => a.Name == action.Name) ?? throw new ArgumentException("Action missing");
            var metadata = result.Properties != null && result.Properties.ContainsKey("metadata") ? result.Properties["metadata"] as Metadata : null;

            result.NextPageAction = GetPageAction(action, functionDefintition, metadata, true);
            result.PreviousPageAction = GetPageAction(action, functionDefintition, metadata, false);
            result.ExportPageAction = GetExportAction(action, functionDefintition);

            result.ExecutedAction = action;
            return result;
        }

        private Models.Input.Action? GetExportAction(Models.Input.Action currentPageAction,
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

        private Models.Input.Action? GetPageAction(Models.Input.Action currentPageAction,
            ActionDescription action, Metadata? metadata, bool isNextPage,
            string pageNumberPropertyName = "pageNumber")
        {
            var hasPageNumberParameter = action.Parameters?.Properties?.Any(p => p.Name == pageNumberPropertyName) ?? false;
            if (!hasPageNumberParameter || metadata == null)
            {
                return null;
            }

            var hasNextPage = metadata.HasNextPage;
            var hasPrevPage = metadata.PageNumber > 1;

            if ((isNextPage && !hasNextPage) || (!isNextPage && !hasPrevPage))
            {
                return null;
            }

            var pageActionEntities = new Dictionary<string, object?>(
                currentPageAction.Entities ?? new Dictionary<string, object?>());

            var newPageNumber = isNextPage ? metadata.PageNumber + 1 : metadata.PageNumber - 1;

            pageActionEntities[pageNumberPropertyName] = newPageNumber;

            return new Models.Input.Action
            {
                Name = currentPageAction.Name,
                Entities = pageActionEntities
            };
        }

        public override IEnumerable<ActionDescription> GetAvailableActions()
        {
            return typeof(SimplicateFunctionsClient).GetTypedFunctionDefinitions(SIMPLICATE);
        }

        private ChatGPTeamsAIClientResponse? ToChatGPTeamsAIResponse<T>(SimplicateDataCollectionResponse<T>? response)
        {
            if (response == null)
            {
                return new ChatGPTeamsAIClientResponse()
                {
                    Type = typeof(T).ToString(),
                    Error = "Something went wrong"
                };
            }

            var dataCard = RenderCard(response.Data);

            return new ChatGPTeamsAIClientResponse()
            {
                Data = response?.Data?.RenderData(),
                DataCard = dataCard,
                TotalItems = response?.Metadata != null ? response?.Metadata?.Count : response?.Data?.Count(),
                TotalPages = response?.Metadata != null ? response?.Metadata?.PageCount : 1,
                CurrentPage = response?.Metadata != null ? response?.Metadata?.PageNumber : 1,
                ItemsPerPage = response?.Metadata != null ? response?.Metadata?.Limit : response?.Data?.Count(),
                Type = typeof(T).ToString(),
                Properties = response?.Metadata != null ? new Dictionary<string, object> { { "metadata", response.Metadata } } : null
            };
        }

        private ChatGPTeamsAIClientResponse? ToChatGPTeamsAIResponse<T>(SimplicateResponseBase<T>? response)
        {
            if (response == null)
            {
                return new ChatGPTeamsAIClientResponse()
                {
                    Type = typeof(T).ToString(),
                    Error = "Something went wrong"
                };
            }

            var dataCard = RenderCard(response.Data);

            return new ChatGPTeamsAIClientResponse()
            {
                Data = response?.Data.RenderData(),
                DataCard = dataCard,
                TotalItems = response?.Metadata?.Count,
                TotalPages = response?.Metadata?.PageCount,
                CurrentPage = response?.Metadata?.PageNumber,
                ItemsPerPage = response?.Metadata?.Limit,
                Type = typeof(T).ToString(),
                Properties = response?.Metadata != null ? new Dictionary<string, object> { { "metadata", response.Metadata } } : null
            };
        }

        private NameValueCollection BuildQueryString(Dictionary<string, string>? filters, string? sort = null)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (!string.IsNullOrEmpty(filter.Value))
                    {
                        queryString[$"q{filter.Key}"] = $"{filter.Value}";
                    }
                }
            }

            if (sort != null)
            {
                queryString[$"sort"] = $"{sort}";
            }

            return queryString;
        }


        private async Task<SimplicateDataCollectionResponse<T>?> FetchSimplicateDataCollection<T>(
                  Dictionary<string, string>? filters,
                  string endpointUrl,
                  long page = 1,
                  string? sort = null)
        {
            if (page <= 0) page = 1;

            var queryString = BuildQueryString(filters, sort);
            long offset = (page - 1) * PAGESIZE;

            queryString["limit"] = PAGESIZE.ToString();
            queryString["offset"] = offset.ToString();
            queryString.Add("metadata", "offset,count,limit");

            if (filters != null)
            {
                /*   var filterQueryString = BuildQueryString(filters);
   if (!string.IsNullOrEmpty(filterQueryString))
   {
       queryString.Add(filterQueryString);
   }

                   foreach (var filter in filters)
                   {
                       if (!string.IsNullOrEmpty(filter.Value))
                       {
                           queryString[$"q{filter.Key}"] = $"{filter.Value}";
                       }
                   }*/
            }

            // Make the request
            var response = await _httpClient.GetAsync($"{endpointUrl}?{queryString}");
            var dsadsa = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return await response.FromJson<SimplicateDataCollectionResponse<T>?>();
            }

            throw new Exception(response.ReasonPhrase);
        }

        private async Task<SimplicateResponseBase<T>?> FetchSimplicateDataItem<T>(
                 string endpointUrl)
        {
            var response = await _httpClient.GetAsync($"{endpointUrl}");

            if (response.IsSuccessStatusCode)
            {
                return await response.FromJson<SimplicateResponseBase<T>>();
            }

            throw new Exception(response.ReasonPhrase);
        }

    }

}

