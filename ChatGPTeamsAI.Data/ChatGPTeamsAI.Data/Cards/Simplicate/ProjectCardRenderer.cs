
using AdaptiveCards;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Cards.Simplicate;

//: ICardRenderer
internal class ProjectCardRenderer 
{
    public AdaptiveCard Render(object item)
    {
        var project = item as Project;

        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));

        card.Body.Add(new AdaptiveTextBlock
        {
            Size = AdaptiveTextSize.Medium,
            Weight = AdaptiveTextWeight.Bolder,
            Text = project?.Name,
            Style = AdaptiveTextBlockStyle.Heading,
            Wrap = true
        });
        var columnSet = new AdaptiveColumnSet();
        card.Body.Add(columnSet);

        var textColumn = new AdaptiveColumn { Width = AdaptiveColumnWidth.Stretch };
        columnSet.Columns.Add(textColumn);

        if (!string.IsNullOrEmpty(project?.ProjectNumber))
        {
            textColumn.Items.Add(new AdaptiveTextBlock
            {
                Weight = AdaptiveTextWeight.Bolder,
                Text = project.ProjectNumber,
                Wrap = true
            });
        }

        if (project?.StartDate != null)
        {
            textColumn.Items.Add(new AdaptiveTextBlock
            {
                Spacing = AdaptiveSpacing.None,
                Text = $"Startdate {{DATE({project.StartDate}, SHORT)}}",
                IsSubtle = true,
                Wrap = true
            });
        }

        if (!string.IsNullOrEmpty(project?.Note))
        {
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = project.Note,
                Wrap = true
            });
        }

        var factSet = new AdaptiveFactSet();
        card.Body.Add(factSet);

        if (project?.OrganizationDetails?.Name != null)
        {
            factSet.Facts.Add(new AdaptiveFact { Title = "Organization:", Value = project.OrganizationDetails.Name });
        }

        if (project?.ProjectManager?.Name != null)
        {
            factSet.Facts.Add(new AdaptiveFact { Title = "Projectmanager:", Value = project.ProjectManager.Name });
        }

        if (project?.EndDate != null)
        {
            factSet.Facts.Add(new AdaptiveFact { Title = "Enddate", Value = $"{{DATE({project.EndDate}, SHORT)}}" });
        }

        if (!string.IsNullOrEmpty(project?.SimplicateUrl))
        {
            card.Actions.Add(new AdaptiveOpenUrlAction
            {
                Title = "View",
                Url = new Uri(project.SimplicateUrl)
            });
        }


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
