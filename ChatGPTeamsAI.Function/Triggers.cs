using System.Net;
using Azure.Storage.Queues.Models;
using ChatGPTeamsAI.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;

namespace ChatGPTeamsAI.Function
{
    public class Triggers
    {
        private readonly ILogger<Triggers> _logger;

        private readonly IConfiguration _config;

        public Triggers(ILogger<Triggers> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        [Function("ChatResponse")]
        public async Task ChatResponse([QueueTrigger("chatresponses")] QueueMessage message)
        {
            var originalActivity = JsonConvert.DeserializeObject<Activity>(message.Body.ToString());
            var chatRequest = JsonConvert.DeserializeObject<ChatRequest>(originalActivity.Value.ToString());

            var apiKey = _config.GetValue<string>("OpenAI:ApiKey") ?? throw new Exception("OpenAI apiKey missing");
            OpenAiOptions options = new()
            {
                ApiKey = apiKey
            };

            using var openAiService = new OpenAIService(options);
            var result = await openAiService.ChatCompletion.CreateCompletion(
                new ChatCompletionCreateRequest()
                {
                    Temperature = (float)chatRequest.Temperture,
                    Messages = new List<ChatMessage>() {
                            new("user", chatRequest.Prompt)
                    }
                }, chatRequest.Backend) ?? throw new Exception("Something went wrong");

            chatRequest.Result = result.Choices?.FirstOrDefault()?.Message.Content;

            originalActivity.Value = chatRequest;

            var responseActivity = new Activity("event")
            {
                Value = originalActivity,
                Name = "LongOperationResponse",
                From = new ChannelAccount()
                {
                    Id = originalActivity.Recipient.Id,
                    Name = originalActivity.Recipient.Name
                }
            };

            var directLineSecret = _config.GetValue<string>("DirectLineSecret");

            using DirectLineClient client = new(directLineSecret);
            client.BaseUri = new Uri("https://europe.directline.botframework.com");

            var conversation = await client.Conversations.StartConversationAsync();

            await client.Conversations.PostActivityAsync(conversation.ConversationId, responseActivity);

        }

        [Function("ActionResponse")]
        public async Task<HttpResponseData> ActionResponse([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData data)
        {
            var bodyString = await data.ReadAsStringAsync();
            var config = JsonConvert.DeserializeObject<ActionRequest>(bodyString);
            var client = new ChatGPTeamsAIData(config.Configuration);
            var result = await client.ExecuteAction(config.Action);
            var response = data.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonConvert.SerializeObject(result));

            return response;
        }

        [Function("AvailableActions")]
        public async Task<HttpResponseData> AvailableActions([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData data)
        {
            var config = await data.ReadFromJsonAsync<Data.Models.Configuration>();  
            var client = new ChatGPTeamsAIData(config);
            var result = client.GetAvailableActions();
            var response = data.CreateResponse(HttpStatusCode.OK);
            
            var json = JsonConvert.SerializeObject(new Actions() {
                ActionDescriptions = result
            });

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(json);
            return response;
        }
    }

    public class ChatRequest
    {
        public required Activity Activity { get; set; }
        public required string Prompt { get; set; }
        public required string RequestId { get; set; }
        public string? Result { get; set; }
        public required string? Backend { get; set; }
        public required double Temperture { get; set; }

    }


    public class ActionRequest
    {
        public required Data.Models.Configuration Configuration { get; set; }
        public required Data.Models.Input.Action Action { get; set; }

    }

    public class Actions
    {
        public required IEnumerable<Data.Models.Output.ActionDescription> ActionDescriptions { get; set; }

    }
}
