using System.Reflection;
using ChatGPTeamsAI.Data.Attributes;
using System.Text.Json;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;
using ChatGPTeamsAI.Data.Models.Output;
using CsvHelper.TypeConversion;

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
               //     var type = listResult.Cast<object>().First().GetType();
                //    var classMap = GetDynamicClassMap(type);

                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        //Delimiter = ";"
                    };

                    using (var writer = new StringWriter())
                    using (var csv = new CsvWriter(writer, config))
                    {
                      //  csv.Context.RegisterClassMap(classMap);
                        csv.WriteRecords(listResult);
                        return writer.ToString();
                    }
                }
            }
            else
            {
                return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            }
        }

        return result?.ToString();
    }

    public static ClassMap GetDynamicClassMap(Type type)
    {
        Type dynamicClassMapType = typeof(DynamicClassMap<>).MakeGenericType(type);
        return (ClassMap)Activator.CreateInstance(dynamicClassMapType);
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
public class DynamicClassMap<T> : ClassMap<T>
{
    public DynamicClassMap()
    {
        var type = typeof(T);
        foreach (var property in type.GetProperties())
        {
            if (property.PropertyType.IsClass
                && property.PropertyType != typeof(string)
                && !typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                foreach (var nestedProperty in property.PropertyType.GetProperties())
                {
                    var name = $"{property.Name}:{nestedProperty.Name}";
                    Map(type, property).Name(name).TypeConverter<NestedPropertyConverter>();
                }
            }
            else if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType)
                     || property.PropertyType == typeof(string))
            {
                Map(type, property);
            }
        }
    }
}

public class NestedPropertyConverter : DefaultTypeConverter
{
    public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value != null)
        {
            var properties = memberMapData.Names.First().Split(':');
            var parentProperty = value.GetType().GetProperty(properties[0]);
            if (parentProperty == null)
            {
                var nestedProperty = value.GetType().GetProperty(properties[1]);
                if (nestedProperty != null)
                {
                    return nestedProperty.GetValue(value)?.ToString();
                }
            }
            else
            {
                var parentValue = parentProperty.GetValue(value);
                if (parentValue != null)
                {
                    var nestedProperty = parentProperty.PropertyType.GetProperty(properties[1]);
                    if (nestedProperty != null)
                    {
                        return nestedProperty.GetValue(parentValue)?.ToString();
                    }
                }
            }
        }

        return base.ConvertToString(value, row, memberMapData);
    }
}
