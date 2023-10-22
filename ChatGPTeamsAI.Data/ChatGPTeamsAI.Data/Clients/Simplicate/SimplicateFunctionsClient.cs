using System.Web;
using ChatGPTeamsAI.Data.Models.Simplicate;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Cards.Simplicate;
using ChatGPTeamsAI.Data.Models.Output;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient : BaseClient
    {
        public const string SIMPLICATE = "Simplicate";

        private readonly HttpClient _httpClient;

        private const int PAGESIZE = 10;

        internal SimplicateFunctionsClient(SimplicateToken token, HttpClient? client = null)
        {
            _httpClient = client ?? new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://{token.Environment}.simplicate.nl/api/v2/");

            _httpClient.DefaultRequestHeaders.Add("Authentication-Key", token.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("Authentication-Secret", token.ApiSecret);

            _cardRenderers.Add(typeof(Project), new ProjectCardRenderer());
        }

        public override async Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Models.Input.Action action)
        {
            var result = await this.ExecuteMethodAsync(action) as ChatGPTeamsAIClientResponse ?? throw new ArgumentException("Something went wrong");
            var functionDefintition = GetAvailableActions().FirstOrDefault(a => a.Name == action.Name) ?? throw new ArgumentException("Action missing");
            var metadata = result.Properties != null && result.Properties.ContainsKey("metadata") ? result.Properties["metadata"] as Metadata : null;

            result.NextPageAction = GetPageAction(action, functionDefintition, metadata, true);
            result.PreviousPageAction = GetPageAction(action, functionDefintition, metadata, false);

            result.ExecutedAction = action;
            return result;
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

            var dataCard = RenderCard(response?.Data);

            return new ChatGPTeamsAIClientResponse()
            {
                Data = dataCard == null ? response?.Data.RenderData() : null,
                DataCard = dataCard,
                TotalItems = response?.Metadata?.Count,
                TotalPages = response?.Metadata?.PageCount,
                CurrentPage = response?.Metadata?.PageNumber,
                ItemsPerPage = response?.Metadata?.Limit,
                Type = typeof(T).ToString(),
                Properties = response?.Metadata != null ? new Dictionary<string, object> { { "metadata", response.Metadata } } : null
            };
        }

        private async Task<SimplicateDataCollectionResponse<T>?> FetchSimplicateDataCollection<T>(
                  Dictionary<string, string>? filters,
                  string endpointUrl,
                  long page = 1)
        {
            if (page <= 0) page = 1;

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            long offset = (page - 1) * PAGESIZE;

            queryString["limit"] = PAGESIZE.ToString();
            queryString["offset"] = offset.ToString();
            queryString.Add("metadata", "offset,count,limit");

            // Add the filters to the query string
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

            // Make the request
            var response = await _httpClient.GetAsync($"{endpointUrl}?{queryString}");

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

