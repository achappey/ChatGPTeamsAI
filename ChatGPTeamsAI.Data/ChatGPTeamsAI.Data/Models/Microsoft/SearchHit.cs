using System.Web;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

public class SearchHit
{
    public string Summary { get; set; }
    public Resource Resource { get; set; }
}

public class Resource
{
    public string _webUrl { get; set; }

    public string WebUrl
    {
        get => HttpUtility.UrlEncode(_webUrl);
        set
        {
            _webUrl = value;
        }
    }
}