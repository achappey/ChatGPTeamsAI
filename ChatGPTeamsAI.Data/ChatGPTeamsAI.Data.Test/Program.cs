﻿using ChatGPTeamsAI.Data;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Output;
using Newtonsoft.Json;

Console.WriteLine("ChatGPTeamsAIData Test");

Settings? settings = ConfigurationExtensions.LoadSettings();

if (settings == null)
{
    throw new Exception("Settings missing");
}

IChatGPTeamsAIData chatGPTeamsAIData = new ChatGPTeamsAIData(settings.ChatGPTeamsAIConfig);

Console.WriteLine("Would you like to test:");
Console.WriteLine("1. All actions");
Console.WriteLine("2. Actions in config/settings only");
Console.WriteLine("3. All Simplicate actions");
Console.WriteLine("4. All Microsoft actions");
string choice = Console.ReadLine();

Console.WriteLine("Retrieving actions...");

var availableActions = chatGPTeamsAIData.GetAvailableActions();

Console.WriteLine($"Actions: {availableActions.Count()} items");

Dictionary<ActionDescription, ActionResponse> errors = new Dictionary<ActionDescription, ActionResponse>();

foreach (var action in availableActions)
{
    if (choice == "2" && (settings.ActionEntities == null || !settings.ActionEntities.ContainsKey(action.Name)))
    {
        Console.WriteLine($"Skipping action: {action.Name}");
        continue;
    }

    if (choice == "3" && (action.Publisher == "Microsoft 365" || action.Name.StartsWith("Export")))
    {
        Console.WriteLine($"Skipping action: {action.Name}");
        continue;
    }

    if (choice == "4" && (action.Publisher == "Simplicate"))
    {
        Console.WriteLine($"Skipping action: {action.Name}");
        continue;
    }

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
        errors.Add(action, response);
    }
    else if (response.Data != null)
    {
        summary = $"Length: {response.Data.Length} chars, Datacard: {response.DataCard != null}";
    }
    else
    {
        summary = "No data or error";
    }

    Console.WriteLine($"Action: {action.Name} Result: {summary}");

    await Task.Delay(1000);
}


foreach (var action in errors)
{
    Console.WriteLine($"Errors action: {action.Key.Name} Publisher: {action.Key.Publisher} Error: {action.Value.Error}");
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
