using System;
using ChatGPTeamsAI.Data.Attributes;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

internal class PlannerTask
{
    public string? Title { get; set; }
    public string? BucketId { get; set; }
    public string? Id { get; set; }
    public string? PlanId { get; set; }
    public PlannerTaskDetails? Details { get; set; }
    public DateTimeOffset? CompletedDateTime { get; set; }
    public DateTimeOffset? DueDateTime { get; set; }
    public int? PercentComplete { get; set; }

}

internal class PlannerBucket
{
    public string? Name { get; set; }
    public string? Id { get; set; }
}

internal class PlannerPlan
{
    [ListColumn]
    [FormColumn]
    public string? Title { get; set; }

    [LinkColumn]
    public string? Url
    {
        get
        {
            return Container?.Url;
        }
        set { }
    }

    public PlannerPlanContainer? Container { get; set; }

    [FormColumn]
    [ListColumn]
    public DateTimeOffset? CreatedDateTime { get; set; }

    [FormColumn]
    public string? Id { get; set; }
}


internal class PlannerPlanContainer
{
    public string? Url { get; set; }
}

internal class PlannerTaskDetails
{
    public string? Description { get; set; }
}