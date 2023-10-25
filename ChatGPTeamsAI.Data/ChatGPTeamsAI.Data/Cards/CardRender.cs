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
            // Als het een lijst is, roep dan DefaultListRender aan
            //return DefaultListRender<typeof(item)(itemsList);

            var itemType = item.GetType().GetGenericArguments()[0];
            // Roep de juiste overload van DefaultListRender aan op basis van het type
            return (AdaptiveCard)typeof(CardRenderer)
                .GetMethod("DefaultListRender")
                .MakeGenericMethod(itemType)
                .Invoke(this, new object[] { itemsList });
        }
        else
        {
            // Als het een enkel item is, roep dan DefaultItemRender aan
            return DefaultItemRender(item);
        }
    }

    public AdaptiveCard DefaultItemRender(object item)
    {
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));

        var typeProperties = item.GetType().GetProperties();
        var columnProperties = typeProperties.Where(p => p.GetCustomAttribute<ColumnNameAttribute>() != null).ToList();

        if (columnProperties.Count < 5)
        {
            var additionalProps = typeProperties.Except(columnProperties).Take(5 - columnProperties.Count).ToList();
            columnProperties.AddRange(additionalProps);
        }

        var factSet = new AdaptiveFactSet();

        foreach (var property in columnProperties)
        {
            var columnNameAttribute = property.GetCustomAttribute<ColumnNameAttribute>();
            var columnName = columnNameAttribute?.Name ?? property.Name;
            var value = property.GetValue(item)?.ToString() ?? " ";

            factSet.Facts.Add(new AdaptiveFact { Title = columnName, Value = value });
        }

        card.Body.Add(factSet);

        return card;
    }

    public AdaptiveCard DefaultListRender<T>(IEnumerable<object> items)
    {
        //var enu = items.getE
        //items as Enumerable
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));
        // var columnSet = new AdaptiveColumnSet();

        var typeProperties = typeof(T).GetProperties();
        var columnProperties = typeProperties.Where(p => p.GetCustomAttribute<ColumnNameAttribute>() != null).ToList();

        // Als er geen eigenschappen zijn met het ColumnNameAttribute, neem dan de eerste drie eigenschappen
        if (!columnProperties.Any())
        {
            columnProperties = typeProperties.Take(3).ToList();
        }

        var columnSet = new AdaptiveColumnSet();

        // Voeg eerst de kolommen en hun koppen toe
        foreach (var property in columnProperties)
        {
            var columnNameAttribute = property.GetCustomAttribute<ColumnNameAttribute>();
            var columnName = columnNameAttribute != null && !string.IsNullOrEmpty(columnNameAttribute.Name) ? columnNameAttribute.Name : property.Name;

            columnSet.Columns.Add(new AdaptiveColumn
            {
                Items =
        {
            new AdaptiveTextBlock
            {
                Text = columnName,
                IsSubtle = true,
                Weight = AdaptiveTextWeight.Bolder
            }
        }
            });
        }

        // Voeg vervolgens de gegevens toe voor elk item in de lijst
        bool isFirstItem = true;

        foreach (var item in items)
        {
            for (int i = 0; i < columnProperties.Count; i++)
            {
                var propertyValue = columnProperties[i].GetValue(item)?.ToString();
                var text = !string.IsNullOrEmpty(propertyValue) ? propertyValue : " ";

                columnSet.Columns[i].Items.Add(new AdaptiveTextBlock
                {
                    Text = text,
                    IsSubtle = true,
                    Separator = isFirstItem
                });
            }

            isFirstItem = false;
        }

        card.Body.Add(columnSet);

        return card;
    }

}
