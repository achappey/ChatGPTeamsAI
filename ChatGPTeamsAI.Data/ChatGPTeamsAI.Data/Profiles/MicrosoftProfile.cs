using AutoMapper;

namespace ChatGPTeamsAI.Data.Profiles;

internal class MicrosoftProfile : Profile
{
    public MicrosoftProfile()
    {
        CreateMap<Microsoft.Graph.User, Models.Microsoft.User>();
        CreateMap<Microsoft.Graph.Group, Models.Microsoft.Group>();
        CreateMap<Microsoft.Graph.Team, Models.Microsoft.Team>();
        CreateMap<Microsoft.Graph.Message, Models.Microsoft.Email>();
        CreateMap<Microsoft.Graph.Recipient, Models.Microsoft.Recipient>();
        CreateMap<Microsoft.Graph.PlannerTask, Models.Microsoft.PlannerTask>();
        CreateMap<Microsoft.Graph.PlannerPlan, Models.Microsoft.PlannerPlan>();
        CreateMap<Microsoft.Graph.PlannerBucket, Models.Microsoft.PlannerBucket>();
        CreateMap<Microsoft.Graph.PlannerPlanContainer, Models.Microsoft.PlannerPlanContainer>();
        CreateMap<Microsoft.Graph.PlannerTaskDetails, Models.Microsoft.PlannerTaskDetails>();
        CreateMap<Microsoft.Graph.Channel, Models.Microsoft.Channel>();
        CreateMap<Microsoft.Graph.Trending, Models.Microsoft.Trending>();
        CreateMap<Microsoft.Graph.ConversationMember, Models.Microsoft.TeamMember>();
        CreateMap<Microsoft.Graph.UsedInsight, Models.Microsoft.UsedInsight>();
        CreateMap<Microsoft.Graph.SharedInsight, Models.Microsoft.SharedInsight>();
        CreateMap<Microsoft.Graph.ResourceVisualization, Models.Microsoft.ResourceVisualization>();
        CreateMap<Microsoft.Graph.ResourceReference, Models.Microsoft.ResourceReference>();
        CreateMap<Microsoft.Graph.EmailAddress, Models.Microsoft.EmailAddress>();
        CreateMap<Microsoft.Graph.Site, Models.Microsoft.Site>();
        CreateMap<Microsoft.Graph.AssignedLicense, Models.Microsoft.AssignedLicense>();
        CreateMap<Microsoft.Graph.ChatMessage, Models.Microsoft.ChatMessage>();
        CreateMap<Microsoft.Graph.ItemBody, Models.Microsoft.ItemBody>();
        CreateMap<Microsoft.Graph.ChatMessageFromIdentitySet, Models.Microsoft.IdentitySet>();
        CreateMap<Microsoft.Graph.IdentitySet, Models.Microsoft.IdentitySet>();
        CreateMap<Microsoft.Graph.Identity, Models.Microsoft.Identity>();
        CreateMap<Microsoft.Graph.SearchHit, Models.Microsoft.SearchHit>();
        CreateMap<Microsoft.Graph.DriveItem, Models.Microsoft.Resource>();
        CreateMap<Microsoft.Graph.TeamworkDevice, Models.Microsoft.TeamworkDevice>();
        CreateMap<Microsoft.Graph.TeamworkHardwareDetail, Models.Microsoft.HardwareDetail>();
        CreateMap<Microsoft.Graph.Message, Models.Microsoft.Resource>();
        CreateMap<Microsoft.Graph.Event, Models.Microsoft.Event>();
        CreateMap<Microsoft.Graph.Device, Models.Microsoft.Device>();
        CreateMap<Microsoft.Graph.ManagedDevice, Models.Microsoft.ManagedDevice>();
        CreateMap<Microsoft.Graph.SitePage, Models.Microsoft.Page>();
        CreateMap<Microsoft.Graph.OnlineMeetingInfo, Models.Microsoft.OnlineMeetingInfo>();
        CreateMap<Microsoft.Graph.Location, Models.Microsoft.Location>();

        CreateMap<Microsoft.Graph.User, Models.Microsoft.User>();

        CreateMap<Microsoft.Graph.ConversationMember, Models.Microsoft.User>();

    }

}