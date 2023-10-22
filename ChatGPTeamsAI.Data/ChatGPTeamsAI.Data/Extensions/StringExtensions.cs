
using System.Globalization;
using System.Text.Json;

using System.Runtime.Serialization;

namespace ChatGPTeamsAI.Data.Extensions;

internal static class StringExtensions
{
    public static void EnsureValidDateFormat(this string dateStr, string expectedFormat = "yyyy-MM-dd HH:mm:ss")
    {
        if (!string.IsNullOrEmpty(dateStr) && !DateTime.TryParseExact(dateStr, expectedFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime temp))
        {
            throw new Exception("Date format is not OK. Expected format: " + expectedFormat);
        }
    }

    public static T? FromJson<T>(this string item)
    {
        return JsonSerializer.Deserialize<T>(item);
    }


    public static string? GetEnumMemberAttributeValue<T>(this T enumValue) where T : Enum
    {
        var type = typeof(T);
        var memberInfos = type.GetMember(enumValue.ToString());
        var enumMemberAttribute = memberInfos[0].GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault() as EnumMemberAttribute;

        return enumMemberAttribute?.Value;
    }
}
