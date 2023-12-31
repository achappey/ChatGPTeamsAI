using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Extensions;

internal static class GraphExtensions
{

    public static string? GetSkipToken(this IList<QueryOption> queryOptions)
    {
        return queryOptions.FirstOrDefault(a => a.Name == "$skiptoken")?.Value;
    }

    public static string? GetSkip(this IList<QueryOption> queryOptions)
    {
        return queryOptions.FirstOrDefault(a => a.Name.EndsWith("skip"))?.Value;
    }

}
