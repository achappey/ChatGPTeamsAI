using AdaptiveCards;
using ChatGPTeamsAI.Cards;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Output;
using ChatGPTeamsAI.Data.Translations;

namespace ChatGPTeamsAI.Data;

internal abstract class BaseClient : IBaseClient
{
    protected readonly Dictionary<Type, object> _cardRenderers = new Dictionary<Type, object>();
    protected readonly CardRenderer defaultRender;

    public abstract IEnumerable<ActionDescription> GetAvailableActions();

    protected string _locale = "en-US";
    protected readonly ITranslationService _translatorService;

    public abstract Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Models.Input.Action action);

    protected BaseClient(ITranslationService? translationService = null)
    {
        _translatorService = translationService ?? new TranslationService(); 
        defaultRender = new CardRenderer(_translatorService);
    }

    public AdaptiveCard? RenderCard(object? data, string? locale = null)
    {
        if (data == null)
        {
            return null;
        }

        var dataType = data.GetType();
        if (_cardRenderers.TryGetValue(dataType, out var rendererObj) && rendererObj is ICardRenderer renderer)
        {
            //      return renderer.Render(data);
        }
        //var defaultRender = new CardRenderer();

        return defaultRender.DefaultRender(data, locale);
        //  base.RenderData(data)

        //   return null;
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