
using AdaptiveCards;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Cards.Simplicate;

internal class ProjectCardRenderer : ICardRenderer
{
    public AdaptiveCard Render(object item)
    {
        var project = item as Project;

        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3))
        {
            Body = {
                    new AdaptiveTextBlock()
                    {
                        Text = project?.Name,
                        Weight = AdaptiveTextWeight.Bolder,
                        Size = AdaptiveTextSize.Medium
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = project?.Note
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = $"Start Date: {project?.StartDate}"
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = $"End Date: {project?.EndDate}"
                    }
                }
        };

        return card;
    }
}
