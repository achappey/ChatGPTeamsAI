using System.Reflection;
using AdaptiveCards;
using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Cards;

internal interface ICardRenderer
{
    // abstract AdaptiveCard Render(object data);
    AdaptiveCard DefaultRender(object item);
}


internal class CardRenderer : ICardRenderer
{
    //AdaptiveCard Render(object data);


    public static AdaptiveTableCell CreateCell(string? value)
    {
        var cell = new AdaptiveTableCell();
        if (!string.IsNullOrEmpty(value))
        {
            cell.Items.Add(new AdaptiveTextBlock
            {
                Text = value,
                HorizontalAlignment = AdaptiveHorizontalAlignment.Left,
                Wrap = true
            });
        }
        else
        {
            cell.Items.Add(new AdaptiveTextBlock
            {
                Text = " ",
                HorizontalAlignment = AdaptiveHorizontalAlignment.Left,
                Wrap = true
            });
        }
        return cell;
    }

    // public abstract AdaptiveCard Render(object data);

    public AdaptiveCard DefaultRender(object item)
    {
        if (item is IEnumerable<object> itemsList)
        {

            if (!itemsList.Any())
            {
                return RenderEmptyListCard();
            }

            return DefaultListRender(itemsList);
        }
        else
        {
            return DefaultItemRender(item);
        }
    }

    public AdaptiveCard RenderEmptyListCard()
    {
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));

        card.Body.Add(new AdaptiveTextBlock
        {
            Text = "No items",
            Weight = AdaptiveTextWeight.Bolder
        });

        return card;
    }


    public AdaptiveCard DefaultItemRender(object item)
    {
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));

        var typeProperties = item.GetType().GetProperties();
        var columnProperties = typeProperties.Where(p => p.GetCustomAttribute<FormColumnAttribute>() != null).ToList();

        if (columnProperties.Count < 5)
        {
            var additionalProps = typeProperties.Except(columnProperties).Take(5 - columnProperties.Count).ToList();
            columnProperties.AddRange(additionalProps);
        }

        var factSet = new AdaptiveFactSet();

        foreach (var property in columnProperties)
        {
            var value = property.GetValue(item)?.ToString() ?? " ";

            factSet.Facts.Add(new AdaptiveFact { Title = property.Name, Value = value });
        }

        card.Body.Add(factSet);

        return card;
    }

    public static AdaptiveCard CreateExportCard(int numberOfItems, string fileName, string url, string name, IDictionary<string, object>? entities = null)
    {
        AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));

        card.Body.Add(new AdaptiveTextBlock
        {
            Text = name,
            Weight = AdaptiveTextWeight.Bolder,
            Size = AdaptiveTextSize.Large
        });

        if (entities != null)
        {
            AdaptiveFactSet entityFactSet = new AdaptiveFactSet();

            foreach (var entity in entities)
            {
                if (entity.Value != null && !string.IsNullOrEmpty(entity.Value.ToString()))
                {
                    entityFactSet.Facts.Add(new AdaptiveFact(entity.Key, entity.Value.ToString()));
                }

            }

            card.Body.Add(entityFactSet);

        }

        AdaptiveFactSet factSet = new AdaptiveFactSet() { Separator = true };
        factSet.Facts.Add(new AdaptiveFact("Items", numberOfItems.ToString()));
        factSet.Facts.Add(new AdaptiveFact("Filename", fileName));
        card.Body.Add(factSet);

        AdaptiveOpenUrlAction urlAction = new AdaptiveOpenUrlAction
        {
            Title = "Open",
            Url = new Uri(url)
        };

        AdaptiveSubmitAction chatAction = new AdaptiveSubmitAction
        {
            Title = "Chat",
            Data = new Data.Models.Input.Action()
            {
                Name = "DocumentChat",
                Entities = new Dictionary<string, object?>() { { url, "" } }
            },
        };

        card.Actions.Add(urlAction);
        card.Actions.Add(chatAction);

        return card;
    }


    public AdaptiveCard DefaultListRender(IEnumerable<object> items)
    {
        var card = InitializeAdaptiveCard();
        var typeProperties = GetTypeProperties(items.First());
        var columnProperties = GetColumnProperties(typeProperties);

        AddHeader(card, columnProperties);

        int toggleId = 1;

        foreach (var item in items)
        {
            var columnSetItem = new AdaptiveColumnSet();
            AddColumnItem(columnSetItem, columnProperties, item, toggleId == 1);
            AddToggleAction(columnSetItem, toggleId);
            card.Body.Add(columnSetItem);

            var toggleContainer = CreateToggleContainer(typeProperties, item, toggleId);
            card.Body.Add(toggleContainer);

            toggleId++;
        }

        return card;
    }

    private AdaptiveCard InitializeAdaptiveCard()
    {
        return new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));
    }

    private PropertyInfo[] GetTypeProperties(object item)
    {
        return item.GetType().GetProperties();
    }

    private List<PropertyInfo> GetColumnProperties(PropertyInfo[] typeProperties)
    {
        return typeProperties.Where(p => p.GetCustomAttribute<ListColumnAttribute>() != null).ToList();
    }

    private void AddHeader(AdaptiveCard card, List<PropertyInfo> columnProperties)
    {
        var container = new AdaptiveContainer() { Style = AdaptiveContainerStyle.Emphasis };
        var columnSetHeader = new AdaptiveColumnSet();
        foreach (var property in columnProperties)
        {
            columnSetHeader.Columns.Add(CreateColumn(property.Name));
        }
        columnSetHeader.Columns.Add(new AdaptiveColumn { Width = "auto" });
        container.Items.Add(columnSetHeader);
        card.Body.Add(container);
    }

    private AdaptiveColumn CreateColumn(string text)
    {
        return new AdaptiveColumn
        {
            Items = { new AdaptiveTextBlock { Text = text, IsSubtle = true, Weight = AdaptiveTextWeight.Bolder } }
        };
    }

    private void AddColumnItem(AdaptiveColumnSet columnSetItem, List<PropertyInfo> columnProperties, object item, bool isFirstItem)
    {
        for (int i = 0; i < columnProperties.Count; i++)
        {
            var propertyValue = columnProperties[i].GetValue(item)?.ToString();
            var text = !string.IsNullOrEmpty(propertyValue) ? propertyValue : " ";

            columnSetItem.Columns.Add(new AdaptiveColumn
            {
                Items = { new AdaptiveTextBlock { Text = text, IsSubtle = true, Separator = isFirstItem } }
            });
        }
    }

    private void AddToggleAction(AdaptiveColumnSet columnSetItem, int toggleId)
    {
        columnSetItem.Columns.Add(new AdaptiveColumn
        {
            Width = "auto",
            Items =
        {
            new AdaptiveImage
            {
                Id = $"chevronDown{toggleId}",
                Url = new Uri("https://adaptivecards.io/content/down.png"),
                PixelWidth = 20,
                IsVisible = true // Initially set to true
            },
            new AdaptiveImage
            {
                Id = $"chevronUp{toggleId}",
                Url = new Uri("https://adaptivecards.io/content/up.png"),
                PixelWidth = 20,
                IsVisible = false // Initially set to false
            }
        },
            SelectAction = new AdaptiveToggleVisibilityAction
            {
                TargetElements = new List<AdaptiveTargetElement>
            {
                new AdaptiveTargetElement($"cardContent{toggleId}"),
                new AdaptiveTargetElement($"chevronDown{toggleId}"),
                new AdaptiveTargetElement($"chevronUp{toggleId}")
            }
            }
        });
    }

    private AdaptiveContainer CreateToggleContainer(PropertyInfo[] typeProperties, object item, int toggleId)
    {
        var toggleContainer = new AdaptiveContainer { Id = $"cardContent{toggleId}", IsVisible = false };
        var factSet = new AdaptiveFactSet();

        var formColumnProperties = typeProperties.Where(p => p.GetCustomAttribute<FormColumnAttribute>() != null).ToList();
        if (!formColumnProperties.Any())
        {
            formColumnProperties = typeProperties.Take(5).ToList();
        }

        foreach (var property in formColumnProperties)
        {
            var value = property.GetValue(item)?.ToString() ?? " ";
            factSet.Facts.Add(new AdaptiveFact { Title = property.Name, Value = value });
        }

        toggleContainer.Items.Add(factSet);

        var linkColumnProperties = typeProperties.Where(p => p.GetCustomAttribute<LinkColumnAttribute>() != null).ToList();
        var chatColumnProperties = typeProperties.Where(p => p.GetCustomAttribute<LinkColumnAttribute>() != null
                && p.GetCustomAttribute<LinkColumnAttribute>().DocumentChat).ToList();

        var columnSet = new AdaptiveColumnSet();

        foreach (var linkColumns in linkColumnProperties)
        {
            var value = linkColumns.GetValue(item)?.ToString() ?? null;

            if (!string.IsNullOrEmpty(value))
            {
                if (!value.StartsWith("http") && !value.StartsWith("mailto:") && !value.StartsWith("tel:"))
                {
                    value = $"https://{value}";
                }

                columnSet.Columns.Add(new AdaptiveColumn()
                {
                    Items = new List<AdaptiveElement>() {
                            new AdaptiveTextBlock
                                {
                                    Text = linkColumns.Name,
                                    HorizontalAlignment = AdaptiveHorizontalAlignment.Right,
                                     Color = AdaptiveTextColor.Accent
                                }
                        },
                    SelectAction = new AdaptiveOpenUrlAction
                    {
                        Url = new Uri(value),
                    }
                });

                if (chatColumnProperties.Contains(linkColumns))
                {
                    columnSet.Columns.Add(new AdaptiveColumn()
                    {
                        Items = new List<AdaptiveElement>() {
                            new AdaptiveTextBlock
                                {
                                    Text = "Chat with " + linkColumns.Name,
                                    HorizontalAlignment = AdaptiveHorizontalAlignment.Right,
                                    Color = AdaptiveTextColor.Accent,
                                }
                        },
                        SelectAction = new AdaptiveSubmitAction
                        {
                            Data = new Data.Models.Input.Action()
                            {
                                Name = "DocumentChat",
                                Entities = new Dictionary<string, object?>() { { value, "" } }
                            },
                        }
                    });
                }
            }
        }

        toggleContainer.Items.Add(columnSet);

        return toggleContainer;
    }

}
