
using AdaptiveCards;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Cards.Simplicate;

internal class ProjectsCardRenderer : ICardRenderer
{
    public AdaptiveCard Render(object item)
    {
        var project = item as IEnumerable<Project>;
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));

        var rows = project.Select(t =>
    {
        return new AdaptiveTableRow()
        {
            Cells = new List<AdaptiveTableCell>()
            {
                t.Name?.CreateCell(),
                t.ProjectNumber?.CreateCell(),
                t.ProjectManager?.Name?.CreateCell(),
            }
        };
    }).ToList();

        card.Body.Add(new AdaptiveTable
        {
            ShowGridLines = false,
            Columns =
                    {
                        new AdaptiveTableColumnDefinition { Width = 1 },
                        new AdaptiveTableColumnDefinition { Width = 1 },
                        new AdaptiveTableColumnDefinition { Width = 1 }
                    },
            Rows = rows
        });



        return card;



        /*

                var card2 = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3))
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

                return card2;*/
    }
}
