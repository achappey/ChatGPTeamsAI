using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class EmailFolder
{
    public string? Id { get; set; }

    [FormColumn]
    [ListColumn]
    public string? DisplayName { get; set; }

    [ListColumn]
    [FormColumn]
    public int UnreadItemCount { get; set; }

    [ListColumn]
    [FormColumn]
    public int TotalItemCount { get; set; }

    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetFolderMessages
    {
        get { return Id != null ? new Dictionary<string, object?>() { { "folderId", Id } } : null; }
        set { }
    }
}
