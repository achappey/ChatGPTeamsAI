using System.Text.Json.Serialization;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class SimplicateResponseBase<T>
{
    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }

    [JsonPropertyName("errors")]
    public IEnumerable<string>? Errors { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    public string Type
    {
        get
        {
            return typeof(T).ToString();
        }
    }

}

internal class SimplicateDataCollectionResponse<T> : SimplicateResponseBase<T>
{
    [JsonPropertyName("data")]
    public new IEnumerable<T>? Data { get; set; }
}

internal class Metadata
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    public int PageNumber
    {
        get
        {
            if (Limit == 0) return 1; // Prevent division by zero
            return 1 + (Offset / Limit);
        }
    }

    public bool HasNextPage
    {
        get
        {
            return (Offset + Limit) < Count;
        }
    }

    public bool HasPaging
    {
        get
        {
            return Limit > 0 && Count > Limit;  // Paginering is aanwezig als Limit is ingesteld en er meer items zijn dan de limiet
        }
    }

    public int PageCount
    {
        get
        {
            if (Limit == 0) return 1;  // Voorkom deling door nul, beschouw alles als één pagina
            return (Count + Limit - 1) / Limit;  // Bereken het totaal aantal pagina's (afgerond naar boven)
        }
    }
}



internal class NewItem
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

}

internal class SimplicateError
{
    public string Message { get; set; } = null!;
}

internal class SimplicateErrorResponse
{
    public IEnumerable<SimplicateError>? Errors { get; set; }
}

internal class SimplicateResponseException : Exception
{
    public SimplicateResponseException(int statusCode, object? value = null) =>
        (StatusCode, Value) = (statusCode, value);

    public int StatusCode { get; }

    public object? Value { get; }
}


