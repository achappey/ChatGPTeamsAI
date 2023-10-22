using AdaptiveCards;

namespace ChatGPTeamsAI.Cards;

internal interface ICardRenderer
{
    AdaptiveCard Render(object data);
}
