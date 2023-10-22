using ChatGPTeamsAI.Data;
using ChatGPTeamsAI.Data.Models;
using Newtonsoft.Json;

Console.WriteLine("ChatGPTeamsAIData Test");

Settings? settings = ConfigurationExtensions.LoadSettings();

if(settings == null) {
    throw new Exception("Settings missing");
}

IChatGPTeamsAIData chatGPTeamsAIData = new ChatGPTeamsAIData(settings.ChatGPTeamsAIConfig);

Console.WriteLine("Retrieving actions...");

var availableActions = chatGPTeamsAIData.GetAvailableActions();

Console.WriteLine($"Actions: {availableActions.Count()} items");

foreach (var action in availableActions)
{
    Console.WriteLine($"Executing action: {action.Name}");

    var entities = settings.ActionEntities != null && settings.ActionEntities.ContainsKey(action.Name) ?
            settings.ActionEntities[action.Name] : null;

    var response = await chatGPTeamsAIData.ExecuteAction(
        new ChatGPTeamsAI.Data.Models.Input.Action()
        {
            Name = action.Name,
            Entities = entities
        });

    string summary;
    if (response.Error != null)
    {
        summary = $"Error: {response.Error}";
    }
    else if (response.Data != null)
    {
        summary = $"Length: {response.Data.Length} chars, Datacard: {response.DataCard != null}, Pagingcard: {response.PagingCard != null}";
    }
    else
    {
        summary = "No data or error";
    }

    Console.WriteLine($"Action: {action.Name} Result: {summary}");

    await Task.Delay(1000);
}

internal class Settings
{
    public required Configuration ChatGPTeamsAIConfig { get; set; }
    public IDictionary<string, Dictionary<string, object?>>? ActionEntities { get; set; }
}


internal static class ConfigurationExtensions
{
    public static Settings? LoadSettings()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
        var json = File.ReadAllText(path);
        var jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
        var settingsJson = jObject != null && jObject.ContainsKey("Settings") ? jObject.GetValue("Settings")?.ToString() : null;
        return settingsJson != null ? JsonConvert.DeserializeObject<Settings>(settingsJson) : null;
    }
}
