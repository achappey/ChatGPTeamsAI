using AdaptiveCards;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Extensions;

internal static class AdaptiveCardExtensions
{

    public static AdaptiveCard? WithPagingButtons(this AdaptiveCard? card,
      Models.Input.Action? nextPage = null, Models.Input.Action? prevPage = null)
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

        return card;
    }

}
