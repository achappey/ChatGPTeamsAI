using AdaptiveCards;
using ChatGPTeamsAI.Cards;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Output;
using ChatGPTeamsAI.Data.Translations;

namespace ChatGPTeamsAI.Data;

internal abstract class BaseClient : IBaseClient
{
    protected readonly Dictionary<Type, object> _cardRenderers = new Dictionary<Type, object>();
    protected readonly CardRenderer defaultRender;

    public abstract IEnumerable<ActionDescription> GetAvailableActions();

    protected readonly ITranslationService _translatorService;

    public abstract Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Models.Input.Action action);

    protected BaseClient(ITranslationService? translationService = null)
    {
        _translatorService = translationService ?? new TranslationService();
        defaultRender = new CardRenderer(_translatorService);
    }


    public AdaptiveCard? RenderNewCard(ActionDescription actionDescription, IDictionary<string, object>? values)
    {
        return defaultRender.CreateNewFormAdaptiveCard(actionDescription, values);
    }

    public AdaptiveCard? RenderCard(object? data)
    {
        if (data == null)
        {
            return null;
        }

        return defaultRender.DefaultRender(data);
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


}