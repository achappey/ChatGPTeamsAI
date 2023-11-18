using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Models;
using Azure.Core.GeoJson;
using Azure.Maps.Routing;

namespace ChatGPTeamsAI.Data.Clients.Azure.Maps
{
    internal partial class AzureMapsFunctionsClient
    {
        [MethodDescription("Routing", "Calculate and retrieve route directions between two points")]
        public Task<ChatGPTeamsAIClientResponse?> GetRouteDirections(
            [ParameterDescription("Start Latitude")] double startLatitude,
            [ParameterDescription("Start Longitude")] double startLongitude,
            [ParameterDescription("End Latitude")] double endLatitude,
            [ParameterDescription("End Longitude")] double endLongitude,
            [ParameterDescription("Travel Mode (car, bicycle, pedestrian, etc.)")] string travelMode,
            [ParameterDescription("Language in EITF format (en-US, nl-NL, etc)")] string? language = null)
        {
            return CalculateRoute(startLatitude, startLongitude, endLatitude, endLongitude, travelMode, language);
        }

        private async Task<ChatGPTeamsAIClientResponse?> CalculateRoute(
                double startLatitude,
                double startLongitude,
                double endLatitude,
                double endLongitude,
                string travelMode,
                string? language = null)
        {
            var routeResult = await _mapsRouteClient.GetDirectionsAsync(
            new RouteDirectionQuery(new List<GeoPosition>() {
            new GeoPosition(startLongitude, startLatitude),
            new GeoPosition(endLongitude, endLatitude)},
                new RouteDirectionOptions()
                {
                    TravelMode = travelMode,
                    Language = language != null ? language : RoutingLanguage.EnglishUsa,
                }));

            var items = routeResult.Value.Routes.Select(_mapper.Map<Models.Azure.Maps.Route>);

            return ToChatGPTeamsAIResponse(items);
        }

    }
}
