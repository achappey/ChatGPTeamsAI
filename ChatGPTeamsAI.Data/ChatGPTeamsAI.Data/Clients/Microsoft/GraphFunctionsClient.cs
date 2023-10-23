using System.Net.Http.Headers;
using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using AutoMapper;
using Microsoft.Graph;
using ChatGPTeamsAI.Data.Profiles;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Output;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient : BaseClient
    {
        private readonly IMapper _mapper;

        private readonly GraphServiceClient _graphClient;

        private const int PAGESIZE = 5;

        public const string MICROSOFT = "Microsoft";

        public GraphFunctionsClient(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MicrosoftProfile())));

            _graphClient = new GraphServiceClient(
               new DelegateAuthenticationProvider(
                   requestMessage =>
                   {
                       requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

                       requestMessage.Headers.Add("Prefer", "outlook.timezone=\"" + TimeZoneInfo.Local.Id + "\"");

                       return Task.CompletedTask;
                   }));
        }

        public override async Task<ChatGPTeamsAIClientResponse?> ExecuteAction(Models.Input.Action action)
        {
            var result = await this.ExecuteMethodAsync(action) as ChatGPTeamsAIClientResponse ?? throw new ArgumentException("Something went wrong");
            var functionDefintition = GetAvailableActions().FirstOrDefault(a => a.Name == action.Name) ?? throw new ArgumentException("Action missing");
            var skipToken = result.Properties != null && result.Properties.ContainsKey("skipToken") ? result.Properties["skipToken"] as string : null;

            result.NextPageAction = GetNextPageAction(action, functionDefintition, skipToken);

            result.ExecutedAction = action;
            return result;
        }

        private Models.Input.Action? GetNextPageAction(Models.Input.Action currentPageAction,
            ActionDescription action, string? skipToken,
            string pageNumberPropertyName = "skipToken")
        {
            var hasPageNumberParameter = action.Parameters?.Properties?.Any(p => p.Name == pageNumberPropertyName) ?? false;
            if (!hasPageNumberParameter || skipToken == null)
            {
                return null;
            }

            var pageActionEntities = new Dictionary<string, object?>(
                currentPageAction.Entities ?? new Dictionary<string, object?>());

            pageActionEntities[pageNumberPropertyName] = skipToken;

            return new Models.Input.Action
            {
                Name = currentPageAction.Name,
                Entities = pageActionEntities
            };
        }

        public override IEnumerable<ActionDescription> GetAvailableActions()
        {
            return typeof(GraphFunctionsClient).GetTypedFunctionDefinitions(MICROSOFT);
        }

        [MethodDescription("Mail", "Get an e-mail by id.")]
        public async Task<Models.Microsoft.Email> GetMail(
            [ParameterDescription("The ID of the e-mail.")] string id)
        {
            var message = await _graphClient.Me.Messages[id].Request().GetAsync();

            return _mapper.Map<Models.Microsoft.Email>(message);
        }

        [MethodDescription("Groups", "Searches for groups based on name or description.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchGroups(
                [ParameterDescription("The group name to filter on.")] string? name = null,
                [ParameterDescription("The description to filter on.")] string? description = null,
                [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            

            string? searchQuery = null;

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(description))
            {
                searchQuery = $"\"displayName:{name ?? "*"}\" OR \"description:{description ?? "*"}\"";
            }

            var filterOptions = new List<QueryOption>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterOptions.Add(new QueryOption("$search", searchQuery));
            }

            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var groups = await _graphClient.Groups.Request(filterOptions)
                            .Header("ConsistencyLevel", "eventual")
                            .GetAsync();

            return ToChatGPTeamsAIResponse(groups.CurrentPage.Select(_mapper.Map<Models.Microsoft.Group>));
        }


        private ChatGPTeamsAIClientResponse? ToChatGPTeamsAIResponse<T>(T? response, string? skipToken = null)
        {
            if (response == null)
            {
                return new ChatGPTeamsAIClientResponse()
                {
                    Type = typeof(T).ToString(),
                    Error = "Something went wrong"
                };
            }

            var dataCard = RenderCard(response);

            return new ChatGPTeamsAIClientResponse()
            {
                Data = dataCard == null ? response?.RenderData() : null,
                DataCard = dataCard,
                Type = typeof(T).ToString(),
                Properties = skipToken != null ? new Dictionary<string, object> { { "skipToken", skipToken } } : null
            };
        }

        // // Gets the user's photo
        // public async Task<PhotoResponse> GetPhotoAsync()
        // {
        //     HttpClient client = new HttpClient();
        //     client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
        //     client.DefaultRequestHeaders.Add("Accept", "application/json");

        //     using (var response = await client.GetAsync("https://graph.microsoft.com/v1.0/me/photo/$value"))
        //     {
        //         if (!response.IsSuccessStatusCode)
        //         {
        //             throw new HttpRequestException($"Graph returned an invalid success code: {response.StatusCode}");
        //         }

        //         var stream = await response.Content.ReadAsStreamAsync();
        //         var bytes = new byte[stream.Length];
        //         stream.Read(bytes, 0, (int)stream.Length);

        //         var photoResponse = new PhotoResponse
        //         {
        //             Bytes = bytes,
        //             ContentType = response.Content.Headers.ContentType?.ToString(),
        //         };

        //         if (photoResponse != null)
        //         {
        //             photoResponse.Base64String = $"data:{photoResponse.ContentType};base64," +
        //                                          Convert.ToBase64String(photoResponse.Bytes);
        //         }

        //         return photoResponse;
        //     }
        // }
      
    }


}