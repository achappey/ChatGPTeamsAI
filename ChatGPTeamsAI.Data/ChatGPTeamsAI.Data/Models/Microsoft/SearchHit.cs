using System.Web;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class SearchHit
{
    public string? Summary { get; set; }
    public Resource? Resource { get; set; }
}

internal class Resource
{
    public string? _webUrl { get; set; }

    public string? WebUrl
    {
        get => HttpUtility.UrlEncode(_webUrl);
        set
        {
            _webUrl = value;
        }
    }
}