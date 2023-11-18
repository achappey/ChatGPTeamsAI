using AutoMapper;

namespace ChatGPTeamsAI.Data.Profiles;

internal class AzureMapsProfile : Profile
{
    public AzureMapsProfile()
    {
        CreateMap<Azure.Maps.Search.Models.SearchAddressResultItem, Models.Azure.Maps.SearchAddress>();
        CreateMap<Azure.Maps.Search.Models.MapsAddress, Models.Azure.Maps.Address>();
        CreateMap<Azure.Maps.Search.Models.PointOfInterest, Models.Azure.Maps.PointOfInterest>();
        CreateMap<Azure.Maps.Routing.Models.RouteSummary, Models.Azure.Maps.RouteSummary>();
        CreateMap<Azure.Maps.Routing.Models.RouteData, Models.Azure.Maps.Route>();
        CreateMap<Azure.Core.GeoJson.GeoPosition, Models.Azure.Maps.Position>();

    }

}