
namespace ChatGPTeamsAI.Data.Translations;

public interface ITranslationService
{
    string Translate(string key);
}

public class TranslationService : ITranslationService
{
    private readonly Dictionary<string, Dictionary<string, string>> translations;

    private const string DefaultLocale = "en-US";
    
    private readonly string? _locale;

    public TranslationService(string? locale = null)
    {
        _locale = locale;
        translations = TranslationData.Data;
    }

    public string Translate(string key)
    {
        if (!string.IsNullOrEmpty(_locale) && translations.ContainsKey(_locale) && translations[_locale].ContainsKey(key))
        {
            return translations[_locale][key];
        }

        if (translations.ContainsKey(DefaultLocale) && translations[DefaultLocale].ContainsKey(key))
        {
            return translations[DefaultLocale][key];
        }

        return key;
    }
}
