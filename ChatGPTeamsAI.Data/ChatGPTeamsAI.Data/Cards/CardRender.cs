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

        card.Actions.Add(urlAction);

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
        var columnSetHeader = new AdaptiveColumnSet();
        foreach (var property in columnProperties)
        {
            columnSetHeader.Columns.Add(CreateColumn(property.Name));
        }
        columnSetHeader.Columns.Add(new AdaptiveColumn { Width = "auto" });
        card.Body.Add(columnSetHeader);
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
            Items = { new AdaptiveImage { Id = $"chevronDown{toggleId}", Url = new Uri("https://adaptivecards.io/content/down.png"), PixelWidth = 20 } },
            SelectAction = new AdaptiveToggleVisibilityAction { TargetElements = new List<AdaptiveTargetElement> { new AdaptiveTargetElement($"cardContent{toggleId}") } }
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
        return toggleContainer;
    }


    public AdaptiveCard DefaultListRender2(IEnumerable<object> items)
    {
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));

        var typeProperties = items.First().GetType().GetProperties();
        var columnProperties = typeProperties.Where(p => p.GetCustomAttribute<ListColumnAttribute>() != null).ToList();

        if (!columnProperties.Any())
        {
            columnProperties = typeProperties.Take(3).ToList();
        }

        var columnSetHeader = new AdaptiveColumnSet();

        foreach (var property in columnProperties)
        {
            columnSetHeader.Columns.Add(new AdaptiveColumn
            {
                Items =
        {
            new AdaptiveTextBlock
            {
                Text = property.Name,
                IsSubtle = true,
                Weight = AdaptiveTextWeight.Bolder
            }
        }
            });
        }

        columnSetHeader.Columns.Add(new AdaptiveColumn { Width = "auto" });
        card.Body.Add(columnSetHeader);

        bool isFirstItem = true;
        int toggleId = 1;

        var formColumnProperties = typeProperties.Where(p => p.GetCustomAttribute<FormColumnAttribute>() != null).ToList();
        var linkColumnProperties = typeProperties.Where(p => p.GetCustomAttribute<LinkColumnAttribute>() != null).ToList();

        if (!formColumnProperties.Any())
        {
            formColumnProperties = typeProperties.Take(5).ToList();
        }

        foreach (var item in items)
        {
            var columnSetItem = new AdaptiveColumnSet();

            for (int i = 0; i < columnProperties.Count; i++)
            {
                columnSetItem.Columns.Add(new AdaptiveColumn());
            }

            for (int i = 0; i < columnProperties.Count; i++)
            {
                var propertyValue = columnProperties[i].GetValue(item)?.ToString();
                var text = !string.IsNullOrEmpty(propertyValue) ? propertyValue : " ";

                columnSetItem.Columns[i].Items.Add(new AdaptiveTextBlock
                {
                    Text = text,
                    IsSubtle = true,
                    Separator = isFirstItem
                });
            }

            isFirstItem = false;

            columnSetItem.Columns.Add(new AdaptiveColumn
            {
                Width = "auto",
                Items = new List<AdaptiveElement>() {
                         new  AdaptiveImage
                        {
                            Id = $"chevronDown{toggleId}",
                            Url = new Uri("https://adaptivecards.io/content/down.png"),
                            PixelWidth = 20
                        }
                        },
                SelectAction = new AdaptiveToggleVisibilityAction
                {
                    TargetElements = new List<AdaptiveTargetElement> { new AdaptiveTargetElement($"cardContent{toggleId}") }
                }
            });

            var toggleContainer = new AdaptiveContainer
            {
                Id = $"cardContent{toggleId}",
                IsVisible = false,
            };

            var factSet = new AdaptiveFactSet();

            foreach (var property in formColumnProperties)
            {
                var value = property.GetValue(item)?.ToString() ?? " ";

                factSet.Facts.Add(new AdaptiveFact
                {
                    Title = property.Name,
                    Value = value
                });
            }

            toggleContainer.Items.Add(factSet);

            var columnSet = new AdaptiveColumnSet();

            foreach (var linkColumns in linkColumnProperties)
            {
                var value = linkColumns.GetValue(item)?.ToString() ?? null;

                if (!string.IsNullOrEmpty(value))
                {
                    if (!value.StartsWith("http"))
                    {
                        value = $"https://{value}";
                    }

                    columnSet.Columns.Add(new AdaptiveColumn()
                    {
                        Items = new List<AdaptiveElement>() {
                            new AdaptiveTextBlock
                                {
                                    Text = linkColumns.Name,
                                    HorizontalAlignment = AdaptiveHorizontalAlignment.Right
                                }
                        },
                        SelectAction = new AdaptiveOpenUrlAction
                        {
                            Url = new Uri(value),
                        }
                    });
                }
            }

            toggleContainer.Items.Add(columnSet);
            card.Body.Add(columnSetItem);
            card.Body.Add(toggleContainer);

            toggleId++;
        }

        return card;
    }
}
