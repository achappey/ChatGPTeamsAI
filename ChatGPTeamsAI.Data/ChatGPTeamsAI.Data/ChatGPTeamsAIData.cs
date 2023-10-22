﻿using System.Text.Json;
using AdaptiveCards;
using ChatGPTeamsAI.Data.Clients.Simplicate;
using ChatGPTeamsAI.Data.Models.Output;
using ChatGPTeamsAI.Data.Models;

namespace ChatGPTeamsAI.Data;

public interface IChatGPTeamsAIData
{
    IEnumerable<ActionDescription> GetAvailableActions();

    Task<ActionResponse> ExecuteAction(Models.Input.Action action);
}

public class ChatGPTeamsAIData : IChatGPTeamsAIData
{
    private readonly Configuration _config;


    private const string MICROSOFT = "Microsoft";

    public ChatGPTeamsAIData(Configuration config)
    {
        _config = config;
    }

    public IEnumerable<ActionDescription> GetAvailableActions()
    {
        if (_config.SimplicateToken == null)
        {
            throw new UnauthorizedAccessException("No Simplicate credentials available");
        }

        var client = new SimplicateFunctionsClient(_config.SimplicateToken);

        return client.GetAvailableActions();
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

          switch (function.Publisher)
          {
              case SimplicateFunctionsClient.SIMPLICATE:
                  actionResult = await ExecuteSimplicateActionAsync(action);
                  break;
              default:
                  throw new InvalidOperationException("Unknown publisher");
          }


          return actionResult != null ? ToDataResponse(actionResult) : new ActionResponse()
          {
              Error = "Something went wrong",
              ExecutedAction = action
          };
      });

    }
    private static ActionResponse ToDataResponse(ChatGPTeamsAIClientResponse clientResponse)
    {
        return new ActionResponse()
        {
            ExecutedAction = clientResponse.ExecutedAction,
            Data = clientResponse.Data,
            PagingCard = RenderPagingCard(clientResponse.ItemsPerPage,
             clientResponse.TotalItems, clientResponse.TotalPages, clientResponse.CurrentPage,
              clientResponse.NextPageAction, clientResponse.PreviousPageAction)?.ToJson(),
            DataCard = clientResponse.DataCard?.ToJson()
        };
    }

    private async Task<ChatGPTeamsAIClientResponse?> ExecuteSimplicateActionAsync(Models.Input.Action action)
    {
        if (_config.SimplicateToken == null)
        {
            throw new UnauthorizedAccessException("No Simplicate credentials available");
        }

        var client = new SimplicateFunctionsClient(_config.SimplicateToken);

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

    public static AdaptiveCard RenderPagingCard(int? itemCount, int? totalItemCount, int? pageCount, int? currentPageCount,
     Models.Input.Action? nextPage = null, Models.Input.Action? prevPage = null)
    {
        var factSet = new AdaptiveFactSet
        {
            Facts = new List<AdaptiveFact>
        {
            new AdaptiveFact { Title = "Item Count", Value = itemCount.ToString() },
            new AdaptiveFact { Title = "Total Item Count", Value = totalItemCount.ToString() },
            new AdaptiveFact { Title = "Page Count", Value = pageCount.ToString() },
            new AdaptiveFact { Title = "Current Page", Value = currentPageCount.ToString() }
        }
        };

        var card = new AdaptiveCard(new AdaptiveSchemaVersion("1.3"))
        {
            Body = new List<AdaptiveElement> { factSet },
            Actions = new List<AdaptiveAction>()
        };

        if (prevPage != null)
        {
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = "Previous",
                Data = prevPage,
           //     AdditionalProperties = new SerializableDictionary<string, object>() { { "Data", JsonSerializer.Serialize(prevPage) } }
            });
        }

        if (nextPage != null)
        {
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = "Next",
                Data = nextPage,
                //AdditionalProperties = new SerializableDictionary<string, object>() { { "Data", JsonSerializer.Serialize(nextPage) } }
            });
        }

        return card;
    }


}
