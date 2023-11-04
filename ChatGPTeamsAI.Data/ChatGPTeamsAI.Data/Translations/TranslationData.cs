namespace ChatGPTeamsAI.Data.Translations;

public class TranslationData
{
    public static Dictionary<string, Dictionary<string, string>> Data = new Dictionary<string, Dictionary<string, string>>
    {
        ["en-US"] = EnglishTranslations.Data,
        ["nl-NL"] = DutchTranslations.Data
    };
}

public static class TranslationKeys
{
    public const string ExecutingAction = "ExecutingAction";
    public const string ProcessingResult = "ProcessingResult";
    public const string Retrying = "Retrying";
    public const string ResultReady = "ResultReady";
    public const string NoItems = "NoItems";
    public const string Items = "Items";
    public const string Filename = "Filename";

    public const string ProjectNumber = "ProjectNumber";
    public const string Name = "Name";
    public const string ProjectManagerName = "ProjectManagerName";
    public const string StatusLabel = "StatusLabel";
    public const string Billable = "Billable";
    public const string StartDate = "StartDate";
    public const string EndDate = "EndDate";
    public const string OrganizationName = "OrganizationName";
    public const string MyOrganizationName = "MyOrganizationName";
    public const string TotalBudget = "TotalBudget";
    public const string TotalSpent = "TotalSpent";
    public const string TotalInvoiced = "TotalInvoiced";
    public const string Note = "Note";
    public const string EmployeeNames = "EmployeeNames";
    public const string CreatedAt = "CreatedAt";
    public const string UpdatedAt = "UpdatedAt";
    public const string PersonName = "PersonName";
    public const string WorkFunction = "WorkFunction";
    public const string IsActive = "IsActive";
    public const string WorkEmail = "WorkEmail";
    public const string WorkPhone = "WorkPhone";
    public const string WorkMobile = "WorkMobile";
    public const string VisitingAddressLocality = "VisitingAddressLocality";
    public const string Email = "Email";
    public const string Phone = "Phone";
    public const string CocCode = "CocCode";
    public const string VatNumber = "VatNumber";
    public const string RelationManagerName = "RelationManagerName";
    public const string RelationTypeLabel = "RelationTypeLabel";
    public const string DebtorMail = "DebtorMail";
    public const string IndustryName = "IndustryName";
    public const string InterestsNames = "InterestsNames";

    public const string EmploymentStatus = "EmploymentStatus";
    public const string CivilStatusLabel = "CivilStatusLabel";
    public const string HourlySalesTariff = "HourlySalesTariff";
    public const string HourlyCostTariff = "HourlyCostTariff";

}
