using ChatGPTeamsAI.Data.Clients.Simplicate;
using ChatGPTeamsAI.Data.Models.Output;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Clients.Microsoft;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Translations;

namespace ChatGPTeamsAI.Data;

public interface IChatGPTeamsAIData
{
    IEnumerable<ActionDescription> GetAvailableActions();

    Task<ActionResponse> ExecuteAction(Models.Input.Action action);
}

public class ChatGPTeamsAIData : IChatGPTeamsAIData
{
    private readonly Configuration _config;
    protected readonly ITranslationService _translatorService;

    public ChatGPTeamsAIData(Configuration config)
    {
        _config = config;
        _translatorService = new TranslationService(config.Locale);
    }

    public IEnumerable<ActionDescription> GetAvailableActions()
    {
        if (_config.SimplicateToken == null)
        {
            throw new UnauthorizedAccessException("No Simplicate credentials available");
        }

        if (_config.GraphApiToken == null)
        {
            throw new UnauthorizedAccessException("No Microsoft Graph credentials available");
        }

        var simplicateClient = new SimplicateFunctionsClient(_config.SimplicateToken, null, _translatorService);
        var simplicateActions = simplicateClient.GetAvailableActions();
        var microsoftClient = new GraphFunctionsClient(_config.GraphApiToken, _translatorService);
        var microsoftActions = microsoftClient.GetAvailableActions();

        return microsoftActions.Concat(simplicateActions).OrderBy(a => a.Category);
    }

    public async Task<ActionResponse> ExecuteAction(Models.Input.Action action)
    {
        return await TryExecuteActionAsync(action.Name, action.Entities, async () =>
        {
            var function = GetAvailableActions().FirstOrDefault(a => a.Name == action.Name);

            if (function == null)
            {
                throw new ArgumentOutOfRangeException("Action not found");
            }

            ChatGPTeamsAIClientResponse? actionResult = null;

            actionResult = function.Publisher switch
            {
                SimplicateFunctionsClient.SIMPLICATE => await ExecuteSimplicateActionAsync(action),
                GraphFunctionsClient.MICROSOFT => await ExecuteMicrosoftActionAsync(action),
                _ => throw new InvalidOperationException("Unknown publisher"),
            };

            return actionResult != null ? action.Name.StartsWith("Export") ? await CreateExport(actionResult)
            : ToDataResponse(actionResult) : new ActionResponse()
            {
                Error = "Something went wrong",
                ExecutedAction = action
            };
        });
    }

    private async Task<ActionResponse> CreateExport(ChatGPTeamsAIClientResponse clientResponse)
    {
        if (_config.GraphApiToken == null)
        {
            throw new UnauthorizedAccessException("No Microsoft Graph credentials available");
        }

        if (clientResponse.Data == null || !clientResponse.TotalItems.HasValue || clientResponse.ExecutedAction == null || clientResponse.ExecutedAction.Name == null)
        {
            throw new Exception("No data available");
        }

        var microsoftClient = new GraphFunctionsClient(_config.GraphApiToken, _translatorService);
        var sanitizedDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        var filename = $"{clientResponse.ExecutedAction?.Name}-{sanitizedDateTime}.csv";
        var webUrl = await microsoftClient.UploadFile(filename, System.Text.Encoding.UTF8.GetBytes(clientResponse.Data));

        return new ActionResponse()
        {
            ExecutedAction = clientResponse.ExecutedAction,
            Data = clientResponse.Data,
            DataCard = microsoftClient.CreateExportCard(clientResponse.TotalItems.Value, filename, webUrl, clientResponse.ExecutedAction.Name)?.ToJson()
        };
    }

    private ActionResponse ToDataResponse(ChatGPTeamsAIClientResponse clientResponse)
    {
        return new ActionResponse()
        {
            ExecutedAction = clientResponse.ExecutedAction,
            Data = clientResponse.Data,
            DataCard = clientResponse.DataCard?.WithButtons(_translatorService, clientResponse.NextPageAction, clientResponse.PreviousPageAction, clientResponse.ExportPageAction)?.ToJson()
        };
    }

    private async Task<ChatGPTeamsAIClientResponse?> ExecuteSimplicateActionAsync(Models.Input.Action action)
    {
        if (_config.SimplicateToken == null)
        {
            throw new UnauthorizedAccessException("No Simplicate credentials available");
        }

        var client = new SimplicateFunctionsClient(_config.SimplicateToken, null, _translatorService);

        return await client.ExecuteAction(action);
    }


    private async Task<ChatGPTeamsAIClientResponse?> ExecuteMicrosoftActionAsync(Models.Input.Action action)
    {
        if (_config.GraphApiToken == null)
        {
            throw new UnauthorizedAccessException("No Microsoft credentials available");
        }

        var client = new GraphFunctionsClient(_config.GraphApiToken, _translatorService);

        return await client.ExecuteAction(action);
    }

    private static async Task<ActionResponse> TryExecuteActionAsync(string name, IDictionary<string, object?>? parameters, Func<Task<ActionResponse>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return new ActionResponse
            {
                Error = ex.Message,
                ExecutedAction = new Models.Input.Action() { Name = name, Entities = parameters }
            };
        }
    }




}
