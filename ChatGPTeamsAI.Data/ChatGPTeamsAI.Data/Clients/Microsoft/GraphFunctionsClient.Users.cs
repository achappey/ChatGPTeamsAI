using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {
        private async Task<ChatGPTeamsAIClientResponse?> RetrieveUsers(string? department, string? displayName, string? jobTitle, string? mail, string? skipToken, int pageSize, bool getAllItems)
        {
            string? searchQuery = null;

            if (!string.IsNullOrEmpty(displayName) || !string.IsNullOrEmpty(mail))
            {
                searchQuery = $"\"displayName:{displayName ?? "*"}\" OR \"mail:{mail ?? "*"}\" OR \"userPrincipalName:{mail ?? "*"}\"";
            }

            string filterQuery = "userType eq 'Member'";

            if (!string.IsNullOrEmpty(department))
            {
                filterQuery += $" and department eq '{department}'";
            }

            if (!string.IsNullOrEmpty(jobTitle))
            {
                filterQuery += $" and jobTitle eq '{jobTitle}'";
            }

            var filterOptions = new List<QueryOption>()
            {
                new QueryOption("$filter", filterQuery)
            };

            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterOptions.Add(new QueryOption("$search", searchQuery));
            }

            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            filterOptions.Add(new QueryOption("$select", "id,externalUserState,userPrincipalName,userType,surname,usageLocation,streetAddress,skills,state,preferredLanguage,postalCode,otherMails,officeLocation,mySite,mobilePhone,mail,jobTitle,lastPasswordChangeDateTime,interests,givenName,employeeHireDate,demployeeId,isplayName,employeeHireDate,department,creationType,createdDateTime,country,companyName,city,businessPhones,birthday,assignedLicenses,accountEnabled,aboutMe"));

            var allUsers = new List<Models.Microsoft.User>();
            IGraphServiceUsersCollectionPage users;

            do
            {
                users = await _graphClient.Users.Request(filterOptions)
                                            .Top(pageSize).Header("ConsistencyLevel", "eventual").GetAsync();

                allUsers.AddRange(users.CurrentPage.Select(_mapper.Map<Models.Microsoft.User>));

                if (!getAllItems) break;

                filterOptions.RemoveAll(option => option.Name == "$skiptoken");
                var nextSkipToken = users.NextPageRequest?.QueryOptions.GetSkipToken();
                if (nextSkipToken != null)
                {
                    filterOptions.Add(new QueryOption("$skiptoken", nextSkipToken));
                }
            } while (users.NextPageRequest?.QueryOptions.GetSkipToken() != null);

            return ToChatGPTeamsAIResponse(allUsers, getAllItems ? null : users.NextPageRequest?.QueryOptions.GetSkipToken());
        }

        [MethodDescription("Export", "Exports a search for users with member type")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportUsers(
            [ParameterDescription("Department")] string? department = null,
            [ParameterDescription("Display name")] string? displayName = null,
            [ParameterDescription("Jobtitle")] string? jobTitle = null,
            [ParameterDescription("Email")] string? mail = null)
        {
            return await RetrieveUsers(department, displayName, jobTitle, mail, null, 999, true);
        }

        [MethodDescription("Users", "Search for users with member type", "ExportUsers")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchUsers([ParameterDescription("Department")] string? department = null,
                                                                        [ParameterDescription("Display name")] string? displayName = null,
                                                                        [ParameterDescription("Jobtitle")] string? jobTitle = null,
                                                                        [ParameterDescription("Email")] string? mail = null,
                                                                        [ParameterDescription("Next page skip token")] string? skipToken = null)
        {

            return await RetrieveUsers(department, displayName, jobTitle, mail, skipToken, PAGESIZE, false);
        }

        [MethodDescription("Users", "Search for guest users")]
        public async Task<ChatGPTeamsAIClientResponse?> SearchGuests([ParameterDescription("The company name to filter on")] string? companyName = null,
                                                                       [ParameterDescription("The display name to filter on")] string? displayName = null,
                                                                       [ParameterDescription("The mail to filter on")] string? mail = null,
                                                                       [ParameterDescription("The next page skip token")] string? skipToken = null)
        {
            string? searchQuery = null;

            if (!string.IsNullOrEmpty(displayName) || !string.IsNullOrEmpty(mail))
            {
                searchQuery = $"\"displayName:{displayName ?? "*"}\" OR \"mail:{mail ?? "*"}\" OR \"userPrincipalName:{mail ?? "*"}\"";
            }

            string filterQuery = "userType eq 'Guest'";
            if (!string.IsNullOrEmpty(companyName))
            {
                filterQuery += $" and startsWith(companyName, '{companyName}')";
            }

            var filterOptions = new List<QueryOption>()
            {
                new QueryOption("$filter", filterQuery)
            };

            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterOptions.Add(new QueryOption("$search", searchQuery));
            }

            if (!string.IsNullOrEmpty(skipToken))
            {
                filterOptions.Add(new QueryOption("$skiptoken", skipToken));
            }

            filterOptions.Add(new QueryOption("$select", "id,externalUserState,userPrincipalName,userType,surname,usageLocation,streetAddress,skills,state,preferredLanguage,postalCode,otherMails,officeLocation,mySite,mobilePhone,mail,jobTitle,lastPasswordChangeDateTime,interests,givenName,employeeHireDate,demployeeId,isplayName,employeeHireDate,department,creationType,createdDateTime,country,companyName,city,businessPhones,birthday,assignedLicenses,accountEnabled,aboutMe"));

            var users = await _graphClient.Users
            .Request(filterOptions)
            .Top(PAGESIZE)
            .Header("ConsistencyLevel", "eventual")
            .GetAsync();

            var items = users.CurrentPage.Select(_mapper.Map<Models.Microsoft.User>);

            return ToChatGPTeamsAIResponse(items, users.NextPageRequest?.QueryOptions.GetSkipToken());

        }

        [MethodDescription("Users", "Gets information about a specific user based on their ID.")]
        public async Task<ChatGPTeamsAIClientResponse?> GetUser(
            [ParameterDescription("The ID of the user.")] string userId)
        {

            var user = await _graphClient.Users[userId].Request().GetAsync();

            return ToChatGPTeamsAIResponse(_mapper.Map<Models.Microsoft.User>(user));
        }
        /*
                [MethodDescription("Creates a user with the specified properties.")]
                public async Task<Models.Graph.User> CreateUser([ParameterDescription("The nickname of the user.")] string nickname,
                                                                [ParameterDescription("The department of the user.")] string department,
                                                                [ParameterDescription("The display name of the user.")] string displayName,
                                                                [ParameterDescription("The given name of the user.")] string givenName,
                                                                [ParameterDescription("The surname of the user.")] string surname,
                                                                [ParameterDescription("The job title of the user.")] string jobTitle,
                                                                [ParameterDescription("The user principal name (email address) of the user.")] string userPrincipalName,
                                                                [ParameterDescription("The password of the user.")] string password)
                {
                    

                    var user = new User
                    {
                        MailNickname = nickname,
                        Department = department,
                        DisplayName = displayName,
                        GivenName = givenName,
                        Surname = surname,
                        JobTitle = jobTitle,
                        UserPrincipalName = userPrincipalName,
                        AccountEnabled = true,
                        PasswordProfile = new PasswordProfile
                        {
                            ForceChangePasswordNextSignIn = true,
                            Password = password
                        }
                    };

                    var createdUser = await _graphClient.Users
                        .Request()
                        .AddAsync(user);

                    return _mapper.Map<Models.Graph.User>(createdUser);
                }

                [MethodDescription("Updates the user's information with the specified properties.")]
                public async Task<Models.Microsoft.User> UpdateUser([ParameterDescription("The ID of the user to update.")] string userId,
                                                                [ParameterDescription("The nickname of the user.")] string nickname = null,
                                                                [ParameterDescription("The department of the user.")] string department = null,
                                                                [ParameterDescription("The display name of the user.")] string displayName = null,
                                                                [ParameterDescription("The given name of the user.")] string givenName = null,
                                                                [ParameterDescription("The surname of the user.")] string surname = null,
                                                                [ParameterDescription("The job title of the user.")] string jobTitle = null,
                                                                [ParameterDescription("Enable or disable the user's account.")] bool? accountEnabled = null)
                {
                    

                    var userToUpdate = new User()
                    {
                    };

                    if (!string.IsNullOrEmpty(nickname)) userToUpdate.MailNickname = nickname;
                    if (!string.IsNullOrEmpty(department)) userToUpdate.Department = department;
                    if (!string.IsNullOrEmpty(displayName)) userToUpdate.DisplayName = displayName;
                    if (!string.IsNullOrEmpty(givenName)) userToUpdate.GivenName = givenName;
                    if (!string.IsNullOrEmpty(surname)) userToUpdate.Surname = surname;
                    if (!string.IsNullOrEmpty(jobTitle)) userToUpdate.JobTitle = jobTitle;
                    if (accountEnabled.HasValue) userToUpdate.AccountEnabled = accountEnabled.Value;

                    await _graphClient.Users[userId]
                        .Request()
                        .UpdateAsync(userToUpdate);

                    var updatedUser = await _graphClient.Users[userId]
                        .Request()
                        .GetAsync();

                    return _mapper.Map<Models.Graph.User>(updatedUser);
                }
        */
    }
}