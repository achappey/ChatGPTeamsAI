using AdaptiveCards;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Cards.Simplicate;

internal class ProjectsCardRenderer : CardRenderer
{
    public override AdaptiveCard Render(object item)
    {
        var projects = item as IEnumerable<Project>;
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));

        var columnSet = new AdaptiveColumnSet();

        // Headers
        columnSet.Columns.Add(new AdaptiveColumn { Items = { new AdaptiveTextBlock { Text = "Number", IsSubtle = true, Weight = AdaptiveTextWeight.Bolder } } });
        columnSet.Columns.Add(new AdaptiveColumn { Items = { new AdaptiveTextBlock { Text = "Name", IsSubtle = true, Weight = AdaptiveTextWeight.Bolder } } });
        columnSet.Columns.Add(new AdaptiveColumn { Items = { new AdaptiveTextBlock { Text = "Projectmanager", IsSubtle = true, Weight = AdaptiveTextWeight.Bolder } } });

        if (projects != null)
        {
            bool isFirstProject = true;

            foreach (var project in projects)
            {
                columnSet.Columns[0].Items.Add(new AdaptiveTextBlock { Text = string.IsNullOrEmpty(project.ProjectNumber) ? project.ProjectNumber : " ", IsSubtle = true, Separator = isFirstProject });
                columnSet.Columns[1].Items.Add(new AdaptiveTextBlock { Text = string.IsNullOrEmpty(project.Name) ? project.Name : " ", IsSubtle = true, Separator = isFirstProject });
                columnSet.Columns[2].Items.Add(new AdaptiveTextBlock { Text = string.IsNullOrEmpty(project.ProjectManager?.Name) ? project.ProjectManager?.Name : " ", IsSubtle = true, Separator = isFirstProject });

                isFirstProject = false;
            }

            card.Body.Add(columnSet);
        }
        /*

        var rows = project.Select(t =>
    {
        return new AdaptiveTableRow()
        {
            Cells = new List<AdaptiveTableCell>()
            {
                CreateCell(t.Name),
                CreateCell(t.ProjectNumber),
                CreateCell(t.ProjectManager?.Name)
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

*/

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
