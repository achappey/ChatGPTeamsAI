using AdaptiveCards;
using ChatGPTeamsAI.Data.Translations;

namespace ChatGPTeamsAI.Data.Extensions;

internal static class AdaptiveCardExtensions
{

    public static AdaptiveCard? WithButtons(this AdaptiveCard? card, ITranslationService translationService,
      Models.Input.Action? nextPage = null, Models.Input.Action? prevPage = null, Models.Input.Action? exportButton = null)
    {
        if (card == null)
        {
            return null;
        }

        if (nextPage == null && prevPage == null && exportButton == null)
        {
            return card;
        }

        if (prevPage != null)
        {
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = translationService.Translate(TranslationKeys.Previous),
                Data = prevPage,
            });
        }

        if (nextPage != null)
        {
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = translationService.Translate(TranslationKeys.Next),
                Data = nextPage
            });
        }

        if (exportButton != null)
        {
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = translationService.Translate(TranslationKeys.Export),
                Data = exportButton
            });
        }

        return card;
    }

}
