using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Output;
using AdaptiveCards;
using ChatGPTeamsAI.Data.Translations;

namespace ChatGPTeamsAI.Data.Clients.Government.NL
{
    internal partial class GovernmentNLFunctionsClient : BaseClient
    {
        private readonly HttpClient _httpClient;

        public const string GOVERNMENT_NL = "Government NL";

        public GovernmentNLFunctionsClient(HttpClient? client = null, ITranslationService? translationService = null) : base(translationService)
        {
            _httpClient = client ?? new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ChatGPTeamsAI");
            _httpClient.BaseAddress = new Uri($"https://opendata.rijksoverheid.nl/v1/infotypes/");
        }

        private static Models.Input.Action? GetExportAction(Models.Input.Action currentPageAction,
                   ActionDescription action)
        {
            if (string.IsNullOrEmpty(action.ExportAction))
            {
                return null;
            }

            var pageActionEntities = new Dictionary<string, object?>(
                         currentPageAction.Entities ?? new Dictionary<string, object?>());

            if (pageActionEntities.ContainsKey("skip"))
            {
                pageActionEntities = pageActionEntities.Where(a => a.Key != "skip").ToDictionary(a => a.Key, a => a.Value);
            }

            return new Models.Input.Action
            {
                Name = action.ExportAction,
                Entities = pageActionEntities
            };
        }


        public override async Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Models.Input.Action action)
        {
            var result = await this.ExecuteMethodAsync(action) as ChatGPTeamsAIClientResponse ?? throw new ArgumentException("Something went wrong");
            var functionDefinition = GetAvailableActions().FirstOrDefault(a => a.Name == action.Name) ?? throw new ArgumentException("Action missing");

            long? skip = result.Properties?.ContainsKey("skip") == true ? result.Properties["skip"] as long? : null;

            result.NextPageAction = GetNextPageAction(action, functionDefinition, skip);
            result.ExportPageAction = GetExportAction(action, functionDefinition);
            result.ExecutedAction = action;
            return result;
        }

        private static Models.Input.Action? GetNextPageAction(Models.Input.Action currentPageAction,
            ActionDescription action, long? skip)
        {
            string? pageProperty = skip != null ? "skip" : null;
            long? pageValue = skip;

            var hasPageProperty = action.Parameters?.Properties?.Any(p => p.Name == pageProperty) ?? false;
            if (!hasPageProperty || pageValue == null || pageProperty == null)
            {
                return null;
            }

            var pageActionEntities = new Dictionary<string, object?>(
                currentPageAction.Entities ?? new Dictionary<string, object?>())
            {
                [pageProperty] = pageValue
            };

            return new Models.Input.Action
            {
                Name = currentPageAction.Name,
                Entities = pageActionEntities
            };
        }


        public override IEnumerable<ActionDescription> GetAvailableActions()
        {
            return typeof(GovernmentNLFunctionsClient).GetTypedFunctionDefinitions(GOVERNMENT_NL);
        }

        private ChatGPTeamsAIClientResponse? ToChatGPTeamsAIResponse<T>(T? response, long? skip = null)
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

            if (skip != null)
            {
                properties = new Dictionary<string, object> { { "skip", skip } };
            }

            return new ChatGPTeamsAIClientResponse()
            {
                Data = response?.RenderData(),
                DataCard = dataCard,
                Type = typeof(T).ToString(),
                Properties = properties
            };
        }

    }


}