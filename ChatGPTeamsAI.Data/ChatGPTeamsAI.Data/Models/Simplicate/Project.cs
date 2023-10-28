

namespace ChatGPTeamsAI.Data.Models.Simplicate;

using System.Text.Json.Serialization;
using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

internal class Project
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("project_manager")]
    [Ignore]
    public Manager? ProjectManager { get; set; }

    [JsonPropertyName("statusLabel")]
    [FormColumn]
    public string? StatusLabel
    {
        get
        {
            return ProjectStatus?.Label;
        }
        set { }
    }

    [JsonPropertyName("project_status")]
    [Ignore]
    public Status? ProjectStatus { get; set; }

    [JsonPropertyName("organization")]
    [Ignore]
    public OrganizationProject? Organization { get; set; }

    [JsonPropertyName("my_organization_profile")]
    [Ignore]
    public MyOrganizationProject? MyOrganization { get; set; }

    [JsonPropertyName("budget")]
    [Ignore]
    public Budget? Budget { get; set; }

    [JsonPropertyName("project_number")]
    [ListColumn]
    public string? ProjectNumber { get; set; }

    [FormColumn]
    [ListColumn]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("billable")]
    [FormColumn]
    public bool Billable { get; set; }

    [JsonPropertyName("start_date")]
    [FormColumn]
    public string? StartDate { get; set; }

    [FormColumn]
    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }


    [ListColumn]
    [JsonPropertyName("projectManagerName")]
    public string? ProjectManagerName
    {
        get
        {
            return ProjectManager?.Name;
        }
        set { }
    }

    [FormColumn]
    [JsonPropertyName("organizationName")]
    public string? OrganizationName
    {
        get
        {
            return Organization?.Name;
        }
        set { }
    }

    [FormColumn]
    [JsonPropertyName("myOganizationName")]
    public string? MyOrganizationName
    {
        get
        {
            return MyOrganization?.Organization?.Name;
        }
        set { }
    }

    [FormColumn]
    [JsonPropertyName("totalBudget")]
    public double? TotalBudget
    {
        get
        {
            return Budget?.Total?.ValueBudget;
        }
        set { }
    }

    [FormColumn]
    [JsonPropertyName("totalSpent")]
    public double? TotalSpent
    {
        get
        {
            return Budget?.Total?.ValueSpent;
        }
        set { }
    }

    [FormColumn]
    [JsonPropertyName("totalInvoiced")]
    public double? TotalInvoiced
    {
        get
        {
            return Budget?.Total?.ValueInvoiced;
        }
        set { }
    }

    [JsonPropertyName("note")]
    [FormColumn]
    public string? Note { get; set; }

    [JsonPropertyName("simplicate_url")]
    [LinkColumn]
    public string? Simplicate { get; set; }

    [Ignore]
    [JsonPropertyName("employees")]
    public IEnumerable<EmployeeProject>? Employees { get; set; }

    [JsonPropertyName("employeeNames")]
    [FormColumn]
    public string? EmployeeNames
    {
        get
        {
            return Employees != null ? string.Join(", ", Employees.Select(a => a.Name)) : string.Empty;
        }
        set { }
    }

    [JsonPropertyName("created_at")]
    [FormColumn]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [FormColumn]
    public string? UpdatedAt { get; set; }


}

internal class OrganizationProject
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

internal class EmployeeProject
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}


internal class MyOrganizationProject
{
    [JsonPropertyName("organization")]
    public OrganizationProject? Organization { get; set; }

}

internal class Manager
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal class Status
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }

}

internal class Budget
{
    [JsonPropertyName("hours")]
    public Hours? Hours { get; set; }

    [JsonPropertyName("costs")]
    public Costs? Costs { get; set; }

    [JsonPropertyName("total")]
    public Total? Total { get; set; }
}

internal class Hours
{
    [JsonPropertyName("amount_budget")]
    public double? AmountBudget { get; set; }

    [JsonPropertyName("amount_spent")]
    public double? AmountSpent { get; set; }

    [JsonPropertyName("value_budget")]
    public double? ValueBudget { get; set; }

    [JsonPropertyName("value_spent")]
    public double? ValueSpent { get; set; }
}

internal class Costs
{
    [JsonPropertyName("value_budget")]
    public double? ValueBudget { get; set; }

    [JsonPropertyName("value_spent")]
    public double? ValueSpent { get; set; }
}

internal class Total
{
    [JsonPropertyName("value_budget")]
    public double? ValueBudget { get; set; }

    [JsonPropertyName("value_spent")]
    public double? ValueSpent { get; set; }

    [JsonPropertyName("value_invoiced")]
    public double? ValueInvoiced { get; set; }
}
