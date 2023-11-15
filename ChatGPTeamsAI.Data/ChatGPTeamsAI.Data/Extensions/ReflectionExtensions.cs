using System.Reflection;
using ChatGPTeamsAI.Data.Attributes;
using System.Text.Json;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;
using ChatGPTeamsAI.Data.Models.Output;
using CsvHelper.TypeConversion;
using System.Text;

namespace ChatGPTeamsAI.Data.Extensions;

internal static class ReflectionExtensions
{
    public static IEnumerable<ActionDescription> GetTypedFunctionDefinitions(this Type type, string publisher)
    {
        var result = new List<ActionDescription>();

        // Get all instance, public, declared-only methods from GraphFunctionsClient
        var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

        // New Filtering
        methods = methods.Where(m => m.GetCustomAttribute<MethodDescriptionAttribute>() != null).ToArray();

        // Iterate through the methods and construct the function definitions
        foreach (var method in methods)
        {
            var functionParameters = new Parameters()
            {
                Properties = method.GetParameters().Select(param => param.ParameterType.MapClrTypeToJsonSchemaType(param)),
                Required = method.GetParameters().Where(param => !string.IsNullOrEmpty(param.Name) && !param.IsOptional).Select(param => param.Name!)
            };

            result.Add(new ActionDescription
            {
                Name = method.Name,
                Publisher = publisher,
                Category = method.GetCustomAttribute<MethodDescriptionAttribute>()?.Category ?? string.Empty,
                Description = method.GetCustomAttribute<MethodDescriptionAttribute>()?.Description ?? string.Empty,
                ExportAction = method.GetCustomAttribute<MethodDescriptionAttribute>()?.ExportAction ?? string.Empty,
                Parameters = functionParameters
            });
        }

        return result;
    }

    public static async Task<object?> ExecuteMethodAsync(this object client, Models.Input.Action action)
    {
        var method = client.GetType().GetMethod(action.Name);
        if (method == null) throw new KeyNotFoundException();

        var parameters = method.GetParameters();
        foreach (var parameter in parameters)
        {
            if (parameter.Name != null && (action.Entities == null || !action.Entities.ContainsKey(parameter.Name)) && !parameter.HasDefaultValue)
            {
                throw new ArgumentException($"Required parameter '{parameter.Name}' is missing.");
            }
        }

        if (action.Entities != null)
        {
            var parameterNames = parameters.Select(p => p.Name).ToList();
            var unknownKeys = action.Entities.Keys.Where(k => !parameterNames.Contains(k)).ToList();
            if (unknownKeys.Any())
            {
                throw new ArgumentException($"Unknown parameters: {string.Join(", ", unknownKeys)}");
            }

            foreach (var key in action.Entities.Keys.ToList())
            {
                if (action.Entities[key] is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Null)
                {
                    action.Entities[key] = null;
                }
            }

        }

        var orderedArguments = method.GetOrderedArguments(action.Entities);
        var methodResult = method.Invoke(client, orderedArguments.ToArray());

        object? taskResult = null;
        if (methodResult is Task task)
        {
            await task;
            if (task.GetType().IsGenericType)
            {
                taskResult = ((dynamic)task).Result;
            }
        }

        return taskResult;
    }

    public static string? RenderData(this object? result)
    {
        if (result == null)
        {
            return string.Empty;
        }

        if (result is string strResult)
        {
            return strResult;
        }
        else if (result != null)
        {
            if (result is System.Collections.IEnumerable listResult)
            {
                if (listResult.Cast<object>().Any())
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        //  ShouldQuote = (args) => true,
                        //   Quote = '\"'
                        Delimiter = ",",

                        // Enclose fields in quotes only if necessary (e.g., when they contain a comma).
                        ShouldQuote = (args) => args.Field.Contains(","),

                        // Use double quotes as the standard quote character.
                        Quote = '\"',

                        // Ensure line breaks within fields are correctly handled.
                        NewLine = Environment.NewLine
                    };

                    using (var stream = new MemoryStream())
                    using (var writer = new StreamWriter(stream, new UTF8Encoding(false)))
                    using (var csv = new CsvWriter(writer, config))
                    {
                        csv.WriteRecords(listResult);
                        writer.Flush();
                        return Encoding.UTF8.GetString(stream.ToArray());
                    }

                }
                else
                {
                    return "No items in the list.";
                }
            }
            else
            {
                return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            }
        }

        return result?.ToString();
    }

    public static List<object?> GetOrderedArguments(this MethodInfo method, IDictionary<string, object?>? arguments)
    {
        var orderedArguments = new List<object?>();

        foreach (var param in method.GetParameters())
        {
            if (arguments == null || (param.Name != null && !arguments.ContainsKey(param.Name)))
            {
                orderedArguments.Add(null);
                continue;
            }

            Type paramType = param.ParameterType;
            if (Nullable.GetUnderlyingType(paramType) is Type underlyingType)
            {
                paramType = underlyingType;
            }

            if (param.Name != null)
            {
                if (paramType.IsEnum && arguments.ContainsKey(param.Name))
                {
                    var enumText = arguments[param.Name]?.ToString();

                    if (enumText != null)
                    {
                        var enumValue = Enum.Parse(paramType, enumText);
                        orderedArguments.Add(enumValue);
                    }
                }
                else if (paramType == typeof(string) && arguments.ContainsKey(param.Name))
                {
                    var arg = arguments[param.Name];

                    if (arg is DateTime time)
                    {
                        arg = time.ToString("o");  // ISO 8601 formaat
                    }

                    orderedArguments.Add(arg);
                }
                else
                {
                    orderedArguments.Add(arguments.ContainsKey(param.Name) ? arguments[param.Name] : null!);
                }
            }
        }

        return orderedArguments;
    }

    public static Property MapClrTypeToJsonSchemaType(this Type type, ParameterInfo paramInfo)
    {
        Type actualType = Nullable.GetUnderlyingType(type) ?? type;
        if (actualType.IsEnum)
        {
            return new Property
            {
                Type = "string",
                Name = paramInfo?.Name ?? string.Empty,
                Description = actualType.GetCustomAttribute<ParameterDescriptionAttribute>()?.Description ?? string.Empty,
                Enum = Enum.GetNames(actualType)
            };
        }

        if (TypeMap.TryGetValue(actualType, out var jsonSchemaType))
        {
            return new Property
            {
                Type = jsonSchemaType,
                Name = paramInfo?.Name ?? string.Empty,
                IsMultiline = paramInfo?.GetCustomAttribute<ParameterDescriptionAttribute>()?.IsMultiline ?? false,
                IsHidden = paramInfo?.GetCustomAttribute<ParameterDescriptionAttribute>()?.IsHidden ?? false,
                Description = paramInfo?.GetCustomAttribute<ParameterDescriptionAttribute>()?.Description ?? string.Empty
            };
        }

        return new Property
        {
            Type = "object",
            Name = paramInfo?.Name ?? string.Empty,
            Description = paramInfo?.GetCustomAttribute<ParameterDescriptionAttribute>()?.Description ?? string.Empty
        };
    }

    private static readonly Dictionary<Type, string> TypeMap = new Dictionary<Type, string>
    {
        { typeof(int), "integer" },
        { typeof(long), "integer" },
        { typeof(float), "number" },
        { typeof(double), "number" },
        { typeof(bool), "boolean" },
        { typeof(string), "string" },
    };
}
