using AdaptiveCards;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Extensions;

internal static class AdaptiveCardExtensions
{

    public static AdaptiveCard? WithButtons(this AdaptiveCard? card,
      Models.Input.Action? nextPage = null, Models.Input.Action? prevPage = null, Models.Input.Action? exportButton = null)
    {
        if (card == null)
        {
            return null;
        }

        if (nextPage == null && prevPage == null)
        {
            return card;
        }

        if (prevPage != null)
        {
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = "Previous",
                Data = prevPage,
            });
        }

        if (nextPage != null)
        {
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = "Next",
                Data = nextPage
            });
        }

          if (exportButton != null)
        {
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = "Export",
                Data = exportButton
            });
        }

        return card;
    }

}
