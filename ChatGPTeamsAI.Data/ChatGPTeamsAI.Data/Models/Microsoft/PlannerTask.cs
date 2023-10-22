using System;

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
    public string? Title { get; set; }

    public PlannerPlanContainer? Container { get; set; }
}


internal class PlannerPlanContainer
{
    public string? Url { get; set; }
}

internal class PlannerTaskDetails
{
    public string? Description { get; set; }
}