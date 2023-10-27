using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Extensions;

internal static class SimplicateExtensions
{
    private const string Offset = "offset";
    private const string Count = "count";
    private const string Limit = "limit";
    private const string Metadata = "metadata";

    public static async Task<IEnumerable<T>> PagedRequest<T>(this HttpClient client, string url, int delayMilliseconds = 500)
    {
        List<T> items = new();
        var uri = new Uri(client.BaseAddress + url);
        uri = uri.AddParameter(Metadata, $"{Offset},{Count},{Limit}");

        int offset = 0;

        SimplicateResponseBase<IEnumerable<T>?>? result;
        do
        {
            uri = uri.AddParameter(Offset, offset.ToString());

            var stopwatch = Stopwatch.StartNew();
            result = await client.SimplicateGetRequest<SimplicateResponseBase<IEnumerable<T>?>>(uri);

            if (result != null && result.Data != null)
            {
                items.AddRange(result.Data);

                offset = result.Metadata!.Offset + result.Metadata.Limit;
            }

            int timeOut = CalculateTimeout(delayMilliseconds, stopwatch.ElapsedMilliseconds);

            await Task.Delay(timeOut);
        }
        while (result?.Metadata!.Count > offset);

        return items;
    }

    public static async Task<T?> SimplicateGetRequest<T>(this HttpClient client, Uri uri)
    {
        return await client.SimplicateRequest<T>(uri, HttpMethod.Get);
    }

    private static int CalculateTimeout(int delayMilliseconds, long elapsedMilliseconds)
    {
        return Math.Max(delayMilliseconds - (int)elapsedMilliseconds, 0);
    }

    private static async Task<T?> SimplicateRequest<T>(this HttpClient client, Uri uri, HttpMethod method, object? bodyContent = null)
    {
        using var httpRequestMessage = new HttpRequestMessage
        {
            Method = method,
            RequestUri = uri,
            Content = bodyContent != null ? new StringContent(JsonSerializer.Serialize(bodyContent), Encoding.UTF8, "application/json") : null
        };

        using var result = await client.SendAsync(httpRequestMessage);

        return await result.HandleSimplicateResponse<T>();
    }

    /// <summary>
    /// Handles the Simplicate API response and returns the deserialized object of type T or throws an appropriate exception.
    /// </summary>
    /// <typeparam name="T">The type of the response object.</typeparam>
    /// <param name="message">The HttpResponseMessage to be handled.</param>
    /// <returns>A task representing the response object of type T, or null if the response content is null.</returns>
    private static async Task<T?> HandleSimplicateResponse<T>(this HttpResponseMessage message)
    {
        switch (message.StatusCode)
        {
            case HttpStatusCode.OK:
                return await message.Content.ReadFromJsonAsync<T>();
            case HttpStatusCode.BadRequest:
                var errors = await message.Content.ReadFromJsonAsync<SimplicateErrorResponse>();

                if (errors != null && errors.Errors != null)
                {
                    throw new SimplicateResponseException((int)message.StatusCode, string.Join(',', errors.Errors.Select(y => y.Message)));
                }
                break;
            case HttpStatusCode.NotFound:
            case HttpStatusCode.Unauthorized:
            default:
                break;
        }

        throw new SimplicateResponseException((int)message.StatusCode);
    }
}