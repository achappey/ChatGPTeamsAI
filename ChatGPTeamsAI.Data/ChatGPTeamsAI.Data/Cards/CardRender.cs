using System.Reflection;
using AdaptiveCards;
using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Models.Output;
using ChatGPTeamsAI.Data.Translations;

namespace ChatGPTeamsAI.Cards;

internal interface ICardRenderer
{
    AdaptiveCard DefaultRender(object item);
}

internal class CardRenderer : ICardRenderer
{
    private readonly ITranslationService _translatorService;

    public CardRenderer(ITranslationService translatorService)
    {
        _translatorService = translatorService;
    }

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
            Text = _translatorService.Translate(TranslationKeys.NoItems),
            Weight = AdaptiveTextWeight.Bolder
        });

        return card;
    }

    public AdaptiveCard CreateNewFormAdaptiveCard(ActionDescription description, IDictionary<string, object> values)
    {
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));

        card.Body.Add(new AdaptiveTextBlock
        {
            Text = _translatorService.Translate(description.Name),
            Weight = AdaptiveTextWeight.Bolder,
            Size = AdaptiveTextSize.Medium
        });

        foreach (var prop in description.Parameters.Properties)
        {
            switch (prop.Type)
            {
                case "boolean":
                    card.Body.Add(new AdaptiveToggleInput
                    {
                        Id = prop.Name,
                        Title = prop.Name,
                        //Value = values.ContainsKey(prop.Name) ? values[prop.Name] : "false",
                        ValueOn = "true",
                        ValueOff = "false"
                    });
                    break;
                case "number":
                    card.Body.Add(new AdaptiveNumberInput
                    {
                        Id = prop.Name,
                        Placeholder = prop.Name,
                        Value = values.ContainsKey(prop.Name) && values[prop.Name] != null ? Convert.ToDouble(values[prop.Name].ToString()) : 0,
                    });
                    break;
                default:
                    card.Body.Add(new AdaptiveTextInput
                    {
                        Id = prop.Name,
                        Placeholder = prop.Name,
                        Value = values.ContainsKey(prop.Name) && values[prop.Name] != null ? values[prop.Name].ToString() : string.Empty,
                    });
                    break;
            }

        }

        card.Actions.Add(new AdaptiveSubmitAction
        {
            Title = _translatorService.Translate(TranslationKeys.Submit),
            Id = description.Name,
        });

        return card;
    }

    public AdaptiveCard DefaultItemRender(object item)
    {
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));

        var typeProperties = item.GetType().GetProperties();

        var container = CreateFormContainer(typeProperties.ToArray(), item);
        card.Body.Add(container);

        return card;
    }

    private void AddLinksToContainer(AdaptiveContainer container, PropertyInfo[] typeProperties, object item)
    {
        var columnSet = new AdaptiveColumnSet();

        var linkColumnProperties = typeProperties.Where(p => p.GetCustomAttribute<LinkColumnAttribute>() != null).ToList();
        var chatColumnProperties = typeProperties.Where(p => p.GetCustomAttribute<LinkColumnAttribute>() != null
                && p.GetCustomAttribute<LinkColumnAttribute>()!.DocumentChat).ToList();

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
                                    Text = _translatorService.Translate(linkColumns.Name),
                                    HorizontalAlignment = AdaptiveHorizontalAlignment.Stretch,
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
                                    Text = _translatorService.Translate(TranslationKeys.AddToChat),
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

        container.Items.Add(columnSet);
    }


    private AdaptiveContainer CreateFormContainer(PropertyInfo[] typeProperties, object item)
    {
        var formContainer = new AdaptiveContainer();

        AddHeaderToContainer(formContainer, typeProperties, item);

        AddFactsToContainer(formContainer, typeProperties, item);

        AddLinksToContainer(formContainer, typeProperties, item);

        return formContainer;
    }

    private void AddHeaderToContainer(AdaptiveContainer container, PropertyInfo[] typeProperties, object item)
    {
        var headerColumnSet = new AdaptiveColumnSet();
        var imageProperty = typeProperties.FirstOrDefault(p => p.GetCustomAttribute<ImageColumnAttribute>() != null);
        var titleProperty = typeProperties.FirstOrDefault(p => p.GetCustomAttribute<TitleColumnAttribute>() != null);
        var updatedProperty = typeProperties.FirstOrDefault(p => p.GetCustomAttribute<UpdatedColumnAttribute>() != null);

        if (imageProperty != null && titleProperty != null)
        {
            var imageColumn = new AdaptiveColumn { Width = "auto" };
            var titleColumn = new AdaptiveColumn { Width = "stretch" };

            var imageUrl = imageProperty.GetValue(item)?.ToString() ?? string.Empty;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                imageColumn.Items.Add(new AdaptiveImage
                {
                    Url = new Uri(imageUrl),
                    AltText = titleProperty.GetValue(item)?.ToString() ?? string.Empty,
                    Size = AdaptiveImageSize.Medium,
                    Style = AdaptiveImageStyle.Default
                });


                var titleText = titleProperty.GetValue(item)?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(titleText))
                {
                    titleColumn.Items.Add(new AdaptiveTextBlock
                    {
                        Text = titleText,
                        Weight = AdaptiveTextWeight.Bolder,
                        Size = AdaptiveTextSize.Large,
                        Wrap = true
                    });
                }

                if (updatedProperty != null)
                {
                    var updatedText = updatedProperty.GetValue(item)?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(updatedText))
                    {
                        titleColumn.Items.Add(new AdaptiveTextBlock
                        {
                            Text = $"{_translatorService.Translate(TranslationKeys.UpdatedAt)} {updatedText}",
                            IsSubtle = true,
                            Size = AdaptiveTextSize.Small,
                            Wrap = true
                        });
                    }
                }

                headerColumnSet.Columns.Add(imageColumn);
                headerColumnSet.Columns.Add(titleColumn);

                container.Items.Add(headerColumnSet);
            }
        }
    }



    public AdaptiveCard DefaultListRender(IEnumerable<object> items)
    {
        var card = InitializeAdaptiveCard();
        var typeProperties = GetTypeProperties(items.First());
        var columnProperties = GetColumnProperties(typeProperties);

        AddHeader(card, columnProperties);

        var formColumnProperties = typeProperties.Where(p => p.GetCustomAttribute<FormColumnAttribute>() != null).ToList();

        int toggleId = 1;

        foreach (var item in items)
        {
            var columnSetItem = new AdaptiveColumnSet() { Separator = toggleId != 1 };
            AddColumnItem(columnSetItem, columnProperties, item, false);

            if (formColumnProperties.Count() > 0)
            {
                AddToggleAction(columnSetItem, toggleId);

            }
            card.Body.Add(columnSetItem);

            if (formColumnProperties.Count() > 0)
            {

                var toggleContainer = CreateToggleContainer(typeProperties, item, toggleId);
                card.Body.Add(toggleContainer);
            }

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
            columnSetHeader.Columns.Add(CreateColumn(_translatorService.Translate(property.Name)));
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
                IsVisible = true
            },
            new AdaptiveImage
            {
                Id = $"chevronUp{toggleId}",
                Url = new Uri("https://adaptivecards.io/content/up.png"),
                PixelWidth = 20,
                IsVisible = false
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

    private void AddFactsToContainer(AdaptiveContainer container, PropertyInfo[] typeProperties, object item)
    {
        var factSet = new AdaptiveFactSet();
        var columnProperties = typeProperties.Where(p => p.GetCustomAttribute<FormColumnAttribute>() != null).ToList();

        foreach (var property in columnProperties)
        {
            var value = property.GetValue(item);

            if (value != null)
            {
                string displayValue = property.PropertyType switch
                {
                    Type when property.PropertyType == typeof(bool) =>
                        (bool)value ? _translatorService.Translate(TranslationKeys.True) : _translatorService.Translate(TranslationKeys.False),
                    _ => value.ToString()
                };

                if (!string.IsNullOrEmpty(displayValue))
                {
                    factSet.Facts.Add(new AdaptiveFact
                    {
                        Title = _translatorService.Translate(property.Name),
                        Value = displayValue
                    });
                }
            }
        }

        container.Items.Add(factSet);
    }

    private AdaptiveContainer CreateToggleContainer(PropertyInfo[] typeProperties, object item, int toggleId)
    {
        var container = CreateFormContainer(typeProperties.ToArray(), item);
        container.Id = $"cardContent{toggleId}";
        container.IsVisible = false;

        return container;

    }

}
