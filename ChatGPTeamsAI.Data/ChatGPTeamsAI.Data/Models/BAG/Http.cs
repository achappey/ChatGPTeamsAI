using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Extensions;

namespace ChatGPTeamsAI.Data.Models.BAG;

internal class BAGResponse
{
    [JsonPropertyName("_embedded")]
    public Embedded? Embedded { get; set; }

    [JsonPropertyName("_links")]
    public PagingLinks? Links { get; set; }
}

internal class Embedded
{
    public List<Address>? Adressen { get; set; }
}



internal class PagingLinks
{
    public Self? Self { get; set; }
    public Self? Next { get; set; }
    public Self? Last { get; set; }

    public int? NextPage
    {
        get
        {
            return Next?.Href.ExtractPageNumber();
        }
    }

    public int? CurrentPage
    {
        get
        {
            return Self?.Href.ExtractPageNumber();
        }
    }

}

internal class Self
{
    public string? Href { get; set; }
}

internal class Building
{
    public string? Href { get; set; }
}