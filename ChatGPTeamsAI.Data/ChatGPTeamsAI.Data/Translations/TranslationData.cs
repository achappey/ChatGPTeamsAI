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
    public const string True = "True";
    public const string False = "False";
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
    public const string Initials = "Initials";
    public const string FirstName = "FirstName";
    public const string FamilyName = "FamilyName";
    public const string Gender = "Gender";
    public const string MobilePhone = "MobilePhone";
    public const string Mail = "Mail";
    public const string PreferredLanguage = "PreferredLanguage";
    public const string AccountEnabled = "AccountEnabled";
    public const string CreatedDateTime = "CreatedDateTime";
    public const string AlternativeMail = "AlternativeMail";
    public const string AssignedLicenseCount = "AssignedLicenseCount";
    public const string DisplayName = "DisplayName";
    public const string Department = "Department";
    public const string Previous = "Previous";
    public const string Next = "Next";
    public const string JobTitle = "JobTitle";
    public const string Export = "Export";
    public const string Subject = "Subject";
    public const string OrganizerName = "OrganizerName";
    public const string StartDateTime = "StartDateTime";
    public const string EndDateTime = "EndDateTime";
    public const string LocationName = "LocationName";
    public const string Content = "Content";
    public const string AddressLocality = "AddressLocality";
    public const string Organizations = "Organizations";
    public const string RelationNumber = "RelationNumber";
    public const string AddToChat = "AddToChat";
    public const string SendMail = "SendMail";
    public const string Call = "Call";
    
    
}
