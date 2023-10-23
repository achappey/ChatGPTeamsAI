using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {

        [MethodDescription("Mail", "Planner|Searches for your Planner tasks based on title or description.")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchMyPlannerTasks(
            [ParameterDescription("The task title to filter on.")] string? title = null,
            [ParameterDescription("The description to filter on.")] string? description = null)
        {
            

            var tasks = await _graphClient.Me.Planner.Tasks
                                .Request()
                                .GetAsync();

            var filteredTasks = tasks.Where(task =>
                (string.IsNullOrEmpty(title) || task.Title.ToLower().Contains(title.ToLower())) &&
                (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(task.Details.Description) || task.Details.Description.ToLower().Contains(description.ToLower()))
            );

            var items = filteredTasks.Select(_mapper.Map<Models.Microsoft.PlannerTask>);

            return ToChatGPTeamsAIResponse(items);
        }
        /*
                [MethodDescription("Mail", "Planner|Creates a new Planner task with the given details.")]
                public async Task<Models.Graph.PlannerTask> CreatePlannerTask(
                    [ParameterDescription("The ID of the Planner to create the task in.")] string plannerId,
                    [ParameterDescription("The ID of the bucket to create the task in.")] string bucketId,
                    [ParameterDescription("The title of the task.")] string title,
                    [ParameterDescription("The description of the task.")] string description = null,
                    [ParameterDescription("The due date of the task.")] DateTime? dueDate = null)
                {
                    

                    var newTask = new Microsoft.Graph.PlannerTask
                    {
                        Title = title,
                        BucketId = bucketId,
                        PlanId = plannerId,
                        Details = new Microsoft.Graph.PlannerTaskDetails { Description = description },
                        DueDateTime = dueDate
                    };

                    var createdTask = await _graphClient.Planner.Tasks
                                            .Request()
                                            .AddAsync(newTask);

                    return _mapper.Map<Models.Graph.PlannerTask>(createdTask);
                }*/

        [MethodDescription("Planner", "Retrieves all user Planners, optionally filtered by a search term.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetAllPlanners(
      [ParameterDescription("The search term to filter planners by title (optional).")] string? searchTerm = null,
      [ParameterDescription("The next page skip token.")] string? skipToken = null)
        {
            

            var filterOptions = new List<QueryOption>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filterOptions.Add(new QueryOption("$filter", $"contains(title, '{searchTerm}')"));
            }

            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            var planners = await _graphClient.Me.Planner.Plans
                                        .Request(filterOptions)
                                        .Top(PAGESIZE)
                                        .GetAsync();

            var items = planners.Select(_mapper.Map<Models.Microsoft.PlannerPlan>);

            return ToChatGPTeamsAIResponse(items, planners.NextPageRequest?.QueryOptions.GetSkipToken());
        }


        [MethodDescription("Planner", "Retrieves the buckets associated with a specific Planner.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetPlannerBuckets(
            [ParameterDescription("The ID of the Planner to get buckets from.")] string plannerId)
        {
            

            var buckets = await _graphClient.Planner.Plans[plannerId].Buckets
                                .Request()
                                .GetAsync();

            var items = buckets.Select(_mapper.Map<Models.Microsoft.PlannerBucket>);

            return ToChatGPTeamsAIResponse(items);
        }

        [MethodDescription("Planner", "Retrieves all tasks from a specified bucket within a Planner.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetTasksFromBucket(
            [ParameterDescription("The ID of the Planner containing the bucket.")] string plannerId,
            [ParameterDescription("The ID of the bucket to retrieve tasks from.")] string bucketId)
        {
            

            var tasks = await _graphClient.Planner.Buckets[bucketId].Tasks
                                .Request()
                                .GetAsync();

            var items = tasks.Select(_mapper.Map<Models.Microsoft.PlannerTask>);

            return ToChatGPTeamsAIResponse(items);
        }
/*
        [MethodDescription("Planner", "Creates a new Planner plan within the specified group.")]
        public async Task<ChatGPTeamsAIClientResponse?> CreatePlanner(
            [ParameterDescription("The ID of the group to create the Planner plan in.")] string groupId,
            [ParameterDescription("The title of the Planner plan.")] string title)
        {
            

            var newPlan = new PlannerPlan
            {
                Title = title,
                Container = new PlannerPlanContainer { Url = $"https://graph.microsoft.com/beta/groups/{groupId}" }
            };

            var createdPlan = await _graphClient.Planner.Plans
                                    .Request()
                                    .AddAsync(newPlan);

            
            return _mapper.Map<Models.Microsoft.PlannerPlan>(createdPlan);
        }

        [MethodDescription("Planner", "Creates a new bucket within the specified Planner plan.")]
        public async Task<Models.Graph.PlannerBucket> CreateBucket(
            [ParameterDescription("The ID of the Planner plan to create the bucket in.")] string planId,
            [ParameterDescription("The name of the bucket.")] string bucketName)
        {
            

            var newBucket = new Microsoft.Graph.PlannerBucket
            {
                Name = bucketName,
                PlanId = planId // PlanId property must be set to the plan ID
            };

            var createdBucket = await _graphClient.Planner.Buckets
                                    .Request()
                                    .AddAsync(newBucket);

            return _mapper.Map<Models.Graph.PlannerBucket>(createdBucket);
        }

        [MethodDescription("Planner", "Copies all details, buckets, and tasks (including checklists) from a source Planner to a target Planner.")]
        public async Task<Models.Response> CopyPlanner(
            [ParameterDescription("The ID of the source Planner to copy from.")] string sourcePlannerId,
            [ParameterDescription("The ID of the target Planner to copy to.")] string targetPlannerId)
        {
            

            // Get all buckets from the source planner
            var sourceBuckets = await _graphClient.Planner.Plans[sourcePlannerId].Buckets.Request().GetAsync();

            // Copy each bucket
            foreach (var sourceBucket in sourceBuckets)
            {
                var newBucket = new Microsoft.Graph.PlannerBucket
                {
                    Name = sourceBucket.Name,
                    PlanId = targetPlannerId
                };

                var createdBucket = await _graphClient.Planner.Buckets.Request().AddAsync(newBucket);

                // Get all tasks from the source bucket
                var sourceTasks = await _graphClient.Planner.Buckets[sourceBucket.Id].Tasks.Request().GetAsync();

                // Copy each task, including details and checklist
                foreach (var sourceTask in sourceTasks)
                {
                    var newTask = new Microsoft.Graph.PlannerTask
                    {
                        Title = sourceTask.Title,
                        BucketId = createdBucket.Id,
                        PlanId = targetPlannerId,
                        //  Details = new Microsoft.Graph.PlannerTaskDetails { Description = sourceTask.Details.Description }, 
                        DueDateTime = sourceTask.DueDateTime
                    };

                    var createdTask = await _graphClient.Planner.Tasks.Request().AddAsync(newTask);

                    // Get task details including checklist
                    var sourceDetails = await _graphClient.Planner.Tasks[sourceTask.Id].Details.Request().GetAsync();

                    // Copy task details including checklist
                    var newDetails = new Microsoft.Graph.PlannerTaskDetails
                    {

                        Description = sourceDetails.Description,
                        PreviewType = PlannerPreviewType.Checklist
                        //  Checklist = sourceDetails.Checklist.ToDictionary(t=> t.Key, t => new PlannerChecklistItem { Title = t.Value.Title }),
                    };
                    //    newDetails.PreviewType = PlannerPreviewType.Checklist;

                    if (newDetails.Checklist == null)
                    {
                        newDetails.Checklist = new PlannerChecklistItems();
                        newDetails.Checklist.AdditionalData = new Dictionary<string, object>();
                    }

                    foreach (var i in sourceDetails.Checklist)
                    {
                        newDetails.Checklist.AdditionalData.Add(Guid.NewGuid().ToString(),
                            new
                            {
                                OdataType = "microsoft.graph.plannerChecklistItem",
                                Title = i.Value.Title,
                                IsChecked = false
                            }
                         );
                    }

                    var request = graphClient.Planner.Tasks[createdTask.Id].Details.Request();
                    var currentDetails = await _graphClient.Planner.Tasks[createdTask.Id].Details.Request().GetAsync();

                    var eTagId = currentDetails.GetEtag();

                    await request.Header("Prefer", "return=representation")
                    .Header("If-Match", eTagId)
                    .UpdateAsync(newDetails);

                    await request.UpdateAsync(newDetails);
                }
            }

            return SuccessResponse();
        }
*/


    }
}