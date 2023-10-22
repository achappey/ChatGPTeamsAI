using System.Text;
using System.Text.Json;
using System.Web;

namespace ChatGPTeamsAI.Data.Extensions;

internal static class HttpExtensions
{

    public static async Task<T?> FromJson<T>(this HttpResponseMessage responseMessage)
    {
        if (responseMessage.IsSuccessStatusCode)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            return content.FromJson<T>();
        }

        throw new Exception(responseMessage.ReasonPhrase);
    }

    public static Uri AddParameter(this Uri url, string paramName, string paramValue)
    {
        if (string.IsNullOrEmpty(paramName)) throw new ArgumentNullException(nameof(paramName));
        if (string.IsNullOrEmpty(paramValue)) throw new ArgumentNullException(nameof(paramValue));

        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query[paramName] = paramValue;
        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }

    public static StringContent PrepareJsonContent<T>(this T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }


}
