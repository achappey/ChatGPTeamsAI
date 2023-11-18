using ChatGPTeamsAI.Data.Extensions;
using AutoMapper;
using ChatGPTeamsAI.Data.Profiles;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Output;
using AdaptiveCards;
using ChatGPTeamsAI.Data.Translations;
using Azure.Maps.Search;
using Azure;
using Azure.Maps.Routing;

namespace ChatGPTeamsAI.Data.Clients.Azure.Maps
{
    internal partial class AzureMapsFunctionsClient : BaseClient
    {
        private readonly IMapper _mapper;

        private readonly MapsSearchClient _mapsSearchClient;

        private readonly MapsRoutingClient _mapsRouteClient;

        private const int PAGESIZE = 5;

        public const string AZURE_MAPS = "Azure Maps";

        public AzureMapsFunctionsClient(string subscriptionKey, ITranslationService? translationService = null) : base(translationService)
        {
            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new ArgumentNullException(nameof(subscriptionKey));
            }

            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AzureMapsProfile())));
            var credential = new AzureKeyCredential(subscriptionKey);
            _mapsSearchClient = new MapsSearchClient(credential);
            _mapsRouteClient = new MapsRoutingClient(credential);
        }

        private Models.Input.Action? GetExportAction(Models.Input.Action currentPageAction,
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


        public AdaptiveCard CreateExportCard(int numberOfItems, string fileName, string url, string name)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));

            card.Body.Add(new AdaptiveTextBlock
            {
                Text = name,
                Weight = AdaptiveTextWeight.Bolder,
                Size = AdaptiveTextSize.Large
            });

            AdaptiveFactSet factSet = new AdaptiveFactSet();
            factSet.Facts.Add(new AdaptiveFact(_translatorService.Translate("Items"), numberOfItems.ToString()));
            factSet.Facts.Add(new AdaptiveFact(_translatorService.Translate("Filename"), fileName));
            card.Body.Add(factSet);

            AdaptiveOpenUrlAction urlAction = new AdaptiveOpenUrlAction
            {
                Title = _translatorService.Translate(TranslationKeys.Open),
                Url = new Uri(url)
            };

            AdaptiveSubmitAction chatAction = new AdaptiveSubmitAction
            {
                Title = _translatorService.Translate(TranslationKeys.AddToChat),
                Data = new Models.Input.Action()
                {
                    Name = "DocumentChat",
                    Entities = new Dictionary<string, object?>() { { url, "" } }
                },
            };

            card.Actions.Add(urlAction);
            card.Actions.Add(chatAction);

            return card;
        }

        private Models.Input.Action? GetNextPageAction(Models.Input.Action currentPageAction,
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
                currentPageAction.Entities ?? new Dictionary<string, object?>());

            pageActionEntities[pageProperty] = pageValue;

            return new Models.Input.Action
            {
                Name = currentPageAction.Name,
                Entities = pageActionEntities
            };
        }


        public override IEnumerable<ActionDescription> GetAvailableActions()
        {
            return typeof(AzureMapsFunctionsClient).GetTypedFunctionDefinitions(AZURE_MAPS);
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