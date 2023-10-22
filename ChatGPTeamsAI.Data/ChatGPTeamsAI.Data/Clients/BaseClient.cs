using AdaptiveCards;
using ChatGPTeamsAI.Cards;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Output;

namespace ChatGPTeamsAI.Data;

internal abstract class BaseClient : IBaseClient
{
    protected readonly Dictionary<Type, object> _cardRenderers = new Dictionary<Type, object>();

    public abstract IEnumerable<ActionDescription> GetAvailableActions();

    public abstract Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Models.Input.Action action);

    public AdaptiveCard? RenderCard(object? data)
    {
        if (data == null)
        {
            return null;
        }

        var dataType = data.GetType();
        if (_cardRenderers.TryGetValue(dataType, out var rendererObj) && rendererObj is ICardRenderer renderer)
        {
            return renderer.Render(data);
        }

        return null;
    }

    public static NoOutputResponse SuccessResponse()
    {
        return new NoOutputResponse
        {
            Status = "success",
            Message = "The function was executed successfully.",
            Timestamp = DateTime.UtcNow
        };
    }

    public static NoOutputResponse ErrorResponse(string error)
    {
        return new NoOutputResponse
        {
            Status = "exception",
            Message = error,
            Timestamp = DateTime.UtcNow
        };
    }


}