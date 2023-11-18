using ChatGPTeamsAI.Data.Attributes;

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


    }

}