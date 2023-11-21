using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class RegionalSettings
{
    [ListColumn]
    [FormColumn("DefaultDisplayLanguage")]
    public string? DefaultDisplayLanguageDisplayName
    {
        get
        {
            return DefaultDisplayLanguage?.DisplayName;
        }
        set { }
    }

    [FormColumn("DefaultTranslationLanguage")]
    public string? DefaultTranslationLanguageDisplayName
    {
        get
        {
            return DefaultTranslationLanguage?.DisplayName;
        }
        set { }
    }

    [FormColumn("DefaultSpeechInputLanguage")]
    public string? DefaultSpeechInputLanguageDisplayName
    {
        get
        {
            return DefaultSpeechInputLanguage?.DisplayName;
        }
        set { }
    }

    [FormColumn("AuthoringLanguages")]
    public string? AuthoringLanguagesDisplayNames
    {
        get
        {
            return AuthoringLanguages != null ? string.Join(", ", AuthoringLanguages.Select(a => a.DisplayName)) : string.Empty;
        }
        set { }
    }

    [Ignore]
    public LanguageInfo? DefaultDisplayLanguage { get; set; }

    [Ignore]
    public List<AuthoringLanguage>? AuthoringLanguages { get; set; }

    [Ignore]
    public LanguageInfo? DefaultTranslationLanguage { get; set; }

    [Ignore]
    public LanguageInfo? DefaultSpeechInputLanguage { get; set; }

    [Ignore]
    public LanguageInfo? DefaultRegionalFormat { get; set; }

    [Ignore]
    public RegionalFormatOverrides? RegionalFormatOverrides { get; set; }

    [Ignore]
    public TranslationPreferences? TranslationPreferences { get; set; }
}

public class LanguageInfo
{
    public string? Locale { get; set; }
    public string? DisplayName { get; set; }
}

public class AuthoringLanguage : LanguageInfo
{
}

public class RegionalFormatOverrides
{
    public string? Calendar { get; set; }
    public string? FirstDayOfWeek { get; set; }
    public string? ShortDateFormat { get; set; }
    public string? LongDateFormat { get; set; }
    public string? ShortTimeFormat { get; set; }
    public string? LongTimeFormat { get; set; }
    public string? TimeZone { get; set; }
}

public class LanguageOverride
{
    public string? LanguageTag { get; set; }
    public string? TranslationBehavior { get; set; }
}

public class TranslationPreferences
{
    public string? TranslationBehavior { get; set; }
    public List<LanguageOverride>? LanguageOverrides { get; set; }
    public List<string>? UntranslatedLanguages { get; set; }
}