using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft
{

    public class TeamsChat
    {

        public string? Id { get; set; }

        [ListColumn]
        [FormColumn]
        public string? Members { get; set; }

        [LinkColumn]
        public string? WebUrl { get; set; }

        [ListColumn]
        [FormColumn]
        public string? Topic { get; set; }

        [Ignore]
        [ActionColumn]
        public IDictionary<string, object>? GetChatMessages
        {
            get { return Id != null ? new Dictionary<string, object>() { { "chatId", Id } } : null; }
            set { }
        }


    }

}