
namespace ChatGPTeamsAI.Data.Translations;

public interface ITranslationService
{
    string Translate(string key, string? locale);
}

public class TranslationService : ITranslationService
{
    private Dictionary<string, Dictionary<string, string>> translations;

    private const string DefaultLocale = "en-US";

    public TranslationService()
    {
        translations = TranslationData.Data;
    }

    public string Translate(string key, string? locale = null)
    {
        if (!string.IsNullOrEmpty(locale) && translations.ContainsKey(locale) && translations[locale].ContainsKey(key))
        {
            return translations[locale][key];
        }

        if (translations.ContainsKey(DefaultLocale) && translations[DefaultLocale].ContainsKey(key))
        {
            return translations[DefaultLocale][key];
        }

        return key;
    }
}
