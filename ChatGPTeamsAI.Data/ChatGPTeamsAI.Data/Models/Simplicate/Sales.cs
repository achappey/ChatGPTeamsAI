

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Sales
{
    [JsonPropertyName("subject")]
    [ListColumn]
    [TitleColumn]
    public string? Subject { get; set; }

    [JsonPropertyName("image")]
    [ImageColumn]
    public string? Image
    {
        get
        {
            if (Contact == null || string.IsNullOrEmpty(Contact.WorkEmail) || !Contact.WorkEmail.Contains('@'))
            {
                return string.Empty;
            }

            var domain = Contact.WorkEmail.Split('@')[1];

            return $"https://logo.clearbit.com/{domain}";
        }
    }

    [ListColumn]
    [FormColumn("General")]
    [JsonPropertyName("responsibleEmployeeName")]
    public string? ResponsibleEmployeeName
    {
        get
        {
            return ResponsibleEmployee?.Name;
        }
        set { }
    }

    [JsonPropertyName("organizationName")]
    [FormColumn("Contact")]
    public string? OrganizationName
    {
        get
        {
            return Organization?.Name;
        }
        set { }
    }

    [JsonPropertyName("personName")]
    [FormColumn("Contact")]
    public string? PersonName
    {
        get
        {
            return Person?.FullName;
        }
        set { }
    }

    [JsonPropertyName("statusLabel")]
    [FormColumn("Status")]
    public string? StatusLabel
    {
        get
        {
            return Status?.Label;
        }
        set { }
    }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("sourceName")]
    [FormColumn("General")]
    public string? SourceName
    {
        get
        {
            return Source?.Name;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("source")]
    public Source? Source { get; set; }

    [JsonPropertyName("progressLabel")]
    [FormColumn("Status")]
    public string? ProgressLabel
    {
        get
        {
            return Progress?.Label;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("progress")]
    public Progress? Progress { get; set; }

    [JsonPropertyName("expected_revenue")]
    [FormColumn("Status")]
    public double? ExpectedRevenue { get; set; }

    [JsonPropertyName("chance_to_score")]
    [FormColumn("Status")]
    public double? ChanceToScore { get; set; }

    [JsonPropertyName("responsible_employee")]
    [Ignore]
    public SalesEmployee? ResponsibleEmployee { get; set; }

    [JsonPropertyName("organization")]
    [Ignore]
    public OrganizationSales? Organization { get; set; }

    [Ignore]
    [JsonPropertyName("person")]
    public PersonSales? Person { get; set; }

    [JsonPropertyName("status")]
    [Ignore]
    public Status? Status { get; set; }

    [JsonPropertyName("start_date")]
    [FormColumn("General")]
    public string? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    [FormColumn("General")]
    public string? EndDate { get; set; }

    [JsonPropertyName("expected_closing_date")]
    [FormColumn("General")]
    public string? ExpectedClosingDate { get; set; }

    [JsonPropertyName("status_updated_at")]
    [FormColumn("Status")]
    public string? StatusUpdatedAt { get; set; }

    [JsonPropertyName("teamNames")]
    [FormColumn("General")]
    public string? TeamNames
    {
        get
        {
            if (Teams != null && Teams.Any())
            {
                return string.Join("\r", Teams.Select(a => a.Name));
            }
            return string.Empty;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("teams")]
    public IEnumerable<Team>? Teams { get; set; }

    [Ignore]
    [JsonPropertyName("contact")]
    public LinkedContact? Contact { get; set; }

    [JsonPropertyName("linkedProjectName")]
    [FormColumn("General")]
    public string? LinkedProjectName
    {
        get
        {
            return LinkedProject?.Name;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("linked_project")]
    public LinkedProject? LinkedProject { get; set; }

    [JsonPropertyName("reasonName")]
    [FormColumn("Status")]
    public string? ReasonName
    {
        get
        {
            return Reason?.Name;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("reason")]
    public Reason? Reason { get; set; }

    [JsonPropertyName("lostToCompetitorName")]
    [FormColumn("Status")]
    public string? LostToCompetitorName
    {
        get
        {
            return LostToCompetitor?.Name;
        }
        set { }
    }

    [Ignore]
    [JsonPropertyName("lost_to_competitor")]
    public Competitor? LostToCompetitor { get; set; }

    [JsonPropertyName("created_at")]
    [FormColumn("General")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn("General")]
    [UpdatedColumn]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("note")]
    [FormColumn("Note")]
    public string? Note { get; set; }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn(category:"General")]
    public string? Simplicate { get; set; }

    [JsonPropertyName("getOrganization")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetOrganization
    {
        get { return Organization != null ? new Dictionary<string, object?>() { { "organizationId", Organization.Id } } : null; }
        set { }
    }
    [JsonPropertyName("getPerson")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetPerson
    {
        get { return Person != null ? new Dictionary<string, object?>() { { "personId", Person.Id } } : null; }
        set { }
    }

    [JsonPropertyName("getProject")]
    [Ignore]
    [ActionColumn]
    public IDictionary<string, object?>? GetProject
    {
        get { return LinkedProject != null ? new Dictionary<string, object?>() { { "projectId", LinkedProject.Id } } : null; }
        set { }
    }
}

internal class PersonSales
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }
}

internal class OrganizationSales
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class SalesEmployee
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("person_id")]
    public string? PersonId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class RevenueGroup
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [ListColumn]
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}


internal class LinkedContact
{
    [JsonPropertyName("person_id")]
    public string? PersonId { get; set; }

    [JsonPropertyName("work_function")]
    public string? WorkFunction { get; set; }

    [JsonPropertyName("work_email")]
    public string? WorkEmail { get; set; }

}


internal class LinkedProject
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

}

internal class Source
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

internal class Reason
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

internal class Competitor
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}


internal class Progress
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }

}