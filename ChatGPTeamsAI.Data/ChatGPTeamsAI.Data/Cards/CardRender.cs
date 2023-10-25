using AdaptiveCards;

namespace ChatGPTeamsAI.Cards;

internal interface ICardRenderer
{
    abstract AdaptiveCard Render(object data);
}


internal abstract class CardRenderer : ICardRenderer
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

    public abstract AdaptiveCard Render(object data);
}
