using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {

        // Create a new channel message.
        [MethodDescription("Teams", "Creates a new channel message.")]
        public async Task<ChatMessage> NewChannelMessage(
            [ParameterDescription("The ID of the team.")] string teamId,
            [ParameterDescription("The ID of the channel.")] string channelId,
            [ParameterDescription("The html content of the message.")] string messageContent)
        {
            var graphClient = GetAuthenticatedClient();

            var newMessage = new ChatMessage
            {
                Body = new ItemBody
                {
                    Content = messageContent,
                    ContentType = BodyType.Html
                }
            };

            return await graphClient.Teams[teamId].Channels[channelId].Messages
                .Request()
                .AddAsync(newMessage);
        }

        [MethodDescription("Teams", "Creates a reply to a specific message in a team's channel.")]
        public async Task<Models.Microsoft.ChatMessage> NewChannelMessageReply(
    [ParameterDescription("The ID of the team.")] string teamId,
    [ParameterDescription("The ID of the channel.")] string channelId,
    [ParameterDescription("The ID of the message.")] string messageId,
    [ParameterDescription("The html content of the reply.")] string replyContent)
        {
            var graphClient = GetAuthenticatedClient();
            var replyMessage = new ChatMessage
            {
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = replyContent
                }
            };

            var newReply = await graphClient.Teams[teamId].Channels[channelId].Messages[messageId].Replies
                .Request()
                .AddAsync(replyMessage);

            return _mapper.Map<Models.Microsoft.ChatMessage>(newReply);
        }

        [MethodDescription("Teams", "Adds a member to a team based on the user's ID and team ID.")]
        public async Task<NoOutputResponse> AddMemberToTeam(
            [ParameterDescription("The ID of the user.")] string userId,
            [ParameterDescription("The ID of the team.")] string teamId)
        {
            var graphClient = GetAuthenticatedClient();

            var member = new AadUserConversationMember
            {
                ODataType = "#microsoft.graph.aadUserConversationMember",
                Roles = new List<string>
                {
                    "member",
                },
                AdditionalData = new Dictionary<string, object>
                {
                    {
                        "user@odata.bind" , $"https://graph.microsoft.com/v1.0/users('{userId}')"
                    },
                },
            };

            await graphClient.Teams[teamId].Members
                .Request()
                .AddAsync(member);

            return SuccessResponse();
        }

        [MethodDescription("Teams", "Adds an owner to a team based on the user's ID and team ID.")]
        public async Task<NoOutputResponse> AddOwnerToTeam(
         [ParameterDescription("The ID of the user.")] string userId,
         [ParameterDescription("The ID of the team.")] string teamId)
        {
            var graphClient = GetAuthenticatedClient();

            var member = new AadUserConversationMember
            {
                ODataType = "#microsoft.graph.aadUserConversationMember",
                Roles = new List<string>
                {
                    "owner",
                },
                AdditionalData = new Dictionary<string, object>
                {
                    {
                        "user@odata.bind" , $"https://graph.microsoft.com/v1.0/users('{userId}')"
                    },
                },
            };

            await graphClient.Teams[teamId].Members
                .Request()
                .AddAsync(member);

            return SuccessResponse();
        }

        [MethodDescription("Team", "Gets details about a specific team, like channel names, team members, etc")]
        public async Task<Models.Microsoft.Team> GetTeam(
            [ParameterDescription("The ID of the team.")] string teamId)
        {
            var graphClient = GetAuthenticatedClient();
            var item = await graphClient.Teams[teamId]
                .Request()
                .Select("id,displayName,description")
                .GetAsync();

            return _mapper.Map<Models.Microsoft.Team>(item);
        }

        [MethodDescription("Teams", "Gets the channels of a specific team based on the ID.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetTeamChannels(
            [ParameterDescription("The ID of the team.")] string teamId)
        {
            var graphClient = GetAuthenticatedClient();
            var items = await graphClient.Teams[teamId].Channels
                .Request()
                .Select("id,displayName,description")
                .GetAsync();

            var result = items.CurrentPage.Select(_mapper.Map<Models.Microsoft.Channel>);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Teams", "Gets the members of a specific team.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetTeamMembers(
           [ParameterDescription("The ID of the team.")] string teamId)
        {
            var graphClient = GetAuthenticatedClient();
            var items = await graphClient.Teams[teamId].Members
                .Request()
                .Select("id,displayName")
                .GetAsync();

            var result = items.CurrentPage.Select(_mapper.Map<Models.Microsoft.TeamMember>);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Teams", "Gets the messages of a specific channel in a team.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetTeamChannelMessages(
      [ParameterDescription("The ID of the team.")] string teamId,
      [ParameterDescription("The ID of the channel.")] string channelId)
        {
            var graphClient = GetAuthenticatedClient();
            var items = await graphClient.Teams[teamId].Channels[channelId].Messages
                .Request()
                .Top(PAGESIZE)
                .GetAsync();

            var result = items.CurrentPage.Select(_mapper.Map<Models.Microsoft.ChatMessage>);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Teams", "Gets the last 25 messages from a specific chat.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetChatMessages(
    [ParameterDescription("The ID of the chat.")] string chatId)
        {
            var graphClient = GetAuthenticatedClient();
            var items = await graphClient.Chats[chatId].Messages
                .Request()
                .Top(25)
                .OrderBy("createdDateTime DESC")
                .GetAsync();

            var result = items.CurrentPage.OrderBy(a => a.CreatedDateTime).Select(_mapper.Map<Models.Microsoft.ChatMessage>);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Teams", "Creates a new message in a specific chat.")]
        public async Task<NoOutputResponse> NewChatMessage(
           [ParameterDescription("The ID of the chat.")] string chatId,
           [ParameterDescription("The html content of the message.")] string content)
        {
            var graphClient = GetAuthenticatedClient();
            var chatMessage = new ChatMessage
            {
                Body = new ItemBody
                {
                    Content = content,
                    ContentType = BodyType.Html,
                },
            };

            await graphClient.Chats[chatId].Messages
                .Request()
                .AddAsync(chatMessage);

            return SuccessResponse();
        }

        [MethodDescription("Teams", "Gets the replies to a specific message in a team's channel.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetChannelMessageReplies(
            [ParameterDescription("The ID of the team.")] string teamId,
            [ParameterDescription("The ID of the channel.")] string channelId,
            [ParameterDescription("The ID of the message.")] string messageId)
        {
            var graphClient = GetAuthenticatedClient();

            var items = await graphClient.Teams[teamId].Channels[channelId].Messages[messageId].Replies
                .Request()
                .Top(PAGESIZE)
                .GetAsync();

            var result = items.CurrentPage.Select(_mapper.Map<Models.Microsoft.ChatMessage>);

            return ToChatGPTeamsAIResponse(result);
        }


        [MethodDescription("Teams", "Updates a channel of a specific team based on the team ID and channel ID.")]
        public async Task<Models.Microsoft.Channel> UpdateTeamChannel(
            [ParameterDescription("The ID of the team.")] string teamId,
            [ParameterDescription("The ID of the channel.")] string channelId,
            [ParameterDescription("The new display name for the channel.")] string newDisplayName,
            [ParameterDescription("The new description for the channel.")] string newDescription)
        {
            var graphClient = GetAuthenticatedClient();

            var updatedChannel = new Channel()
            {
                DisplayName = newDisplayName,
                Description = newDescription,
                Id = channelId
            };

            var channel = await graphClient.Teams[teamId].Channels[channelId]
                .Request()
                .UpdateAsync(updatedChannel);

            return _mapper.Map<Models.Microsoft.Channel>(channel);
        }

        [MethodDescription("Teams", "Updates a team based on the team ID.")]
        public async Task<Models.Microsoft.Team> UpdateTeam(
    [ParameterDescription("The ID of the team.")] string teamId,
    [ParameterDescription("The new display name for the team.")] string newDisplayName,
    [ParameterDescription("The new description for the team.")] string newDescription)
        {
            var graphClient = GetAuthenticatedClient();

            var updatedTeam = new Team()
            {
                DisplayName = newDisplayName,
                Description = newDescription,
                Id = teamId
            };

            var team = await graphClient.Teams[teamId]
                .Request()
                .UpdateAsync(updatedTeam);

            return _mapper.Map<Models.Microsoft.Team>(team);
        }


        [MethodDescription("Teams", "Retrieves all TeamsRooms devices.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetTeamworkDevices()
        {
            var graphClient = GetAuthenticatedClient();
            var devices = await graphClient.Teamwork.Devices.Request().GetAsync();
            var result = devices.CurrentPage.Select(_mapper.Map<Models.Microsoft.TeamworkDevice>);

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Teams", "Restarts a specific TeamsRooms device by device ID.")]
        public async Task<NoOutputResponse> RestartTeamworkDevice(
            [ParameterDescription("The ID of the teamwork device to restart.")] string deviceId)
        {
            var graphClient = GetAuthenticatedClient();

            await graphClient.Teamwork.Devices[deviceId].Restart().Request().PostAsync();

            return SuccessResponse();
        }

        [MethodDescription("Teams", "Creates a new channel within the specified Microsoft Teams.")]
        public async Task<Models.Microsoft.Channel> CreateChannel(
            [ParameterDescription("The ID of the Microsoft Teams team to create the channel in.")] string teamId,
            [ParameterDescription("The name of the channel.")] string channelName,
            [ParameterDescription("The description of the channel.")] string? channelDescription = null)
        {
            var graphClient = GetAuthenticatedClient();

            var newChannel = new Channel
            {
                DisplayName = channelName,
                Description = channelDescription
            };

            var createdChannel = await graphClient.Teams[teamId].Channels
                                    .Request()
                                    .AddAsync(newChannel);

            return _mapper.Map<Models.Microsoft.Channel>(createdChannel);
        }

    }
}