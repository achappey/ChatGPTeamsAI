using System;

namespace ChatGPTeamsAI.Data.Models.Microsoft;

public class PlannerTask
{
    public string Title { get; set; }
    public string BucketId { get; set; }
    public string Id { get; set; }
    public string PlanId { get; set; }
    public PlannerTaskDetails Details { get; set; }
    public DateTimeOffset? CompletedDateTime { get; set; }
    public DateTimeOffset? DueDateTime { get; set; }
    public int? PercentComplete { get; set; }
    
}

public class PlannerBucket
{
    public string Name { get; set; }
    public string Id { get; set; }
}

public class PlannerPlan
{
    public string Title { get; set; }

    public PlannerPlanContainer Container { get; set; }
}


public class PlannerPlanContainer
{
    public string Url { get; set; }
}

public class PlannerTaskDetails
{
    public string Description { get; set; }
}