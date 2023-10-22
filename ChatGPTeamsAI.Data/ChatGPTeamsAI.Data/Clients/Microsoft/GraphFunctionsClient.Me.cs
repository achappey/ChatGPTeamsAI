﻿using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Models;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace ChatGPTeamsAI.Data.Clients.Microsoft
{
    internal partial class GraphFunctionsClient
    {
        [MethodDescription("Mail", "Sends an email using the Microsoft Graph API")]
        public async Task<NoOutputResponse> SendMail([ParameterDescription("The email addresses to send the email to seperated by ;")] string toAddresses,
            [ParameterDescription("The email addresses to cc the email to seperated by ;")] string ccAddresses,
            [ParameterDescription("The subject of the email")] string subject,
           [ParameterDescription("HTML content")] string html)
        {
            if (string.IsNullOrWhiteSpace(toAddresses))
            {
                throw new ArgumentNullException(nameof(toAddresses));
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(html))
            {
                throw new ArgumentNullException(nameof(html));
            }

            var graphClient = GetAuthenticatedClient();
            var recipients = toAddresses.Split(";").Select(a =>
            {
                return new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = a,
                    }
                };
            });

            var ccRecipients = !string.IsNullOrEmpty(ccAddresses) ? ccAddresses.Split(";").Select(a =>
            {
                return new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = a,
                    }
                };
            }) : null;

            // Create the message.
            var email = new Message
            {
                Body = new ItemBody
                {
                    Content = html,
                    ContentType = BodyType.Html,
                },
                Subject = subject,
                SentDateTime = DateTime.UtcNow,
                ToRecipients = recipients,
                CcRecipients = ccRecipients,
                SingleValueExtendedProperties = new MessageSingleValueExtendedPropertiesCollectionPage
            {
                new SingleValueLegacyExtendedProperty()
                {
                    Id = "SystemTime 0x3FEF",
                    Value = DateTime.UtcNow.AddMinutes(1).ToString("o")
                }
            }
            };

            await graphClient.Me.SendMail(email, true)
                    .Request()
                    .PostAsync();

            return SuccessResponse();
        }

        [MethodDescription("Mail", "Replies an email using the Microsoft Graph API")]
        public async Task<NoOutputResponse> ReplyMail(
            [ParameterDescription("The ID of the e-mail.")] string id,
            [ParameterDescription("The email addresses to send the email to seperated by ;")] string toAddresses,
            [ParameterDescription("The comment on the email")] string comment)
        {
            if (string.IsNullOrWhiteSpace(toAddresses))
            {
                throw new ArgumentNullException(nameof(toAddresses));
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentNullException(nameof(comment));
            }

            var graphClient = GetAuthenticatedClient();
            var recipients = toAddresses.Split(";").Select(a =>
            {
                return new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = a,
                    }
                };
            });

            var email = new Message
            {
                ToRecipients = recipients,
                SingleValueExtendedProperties = new MessageSingleValueExtendedPropertiesCollectionPage
            {
                new SingleValueLegacyExtendedProperty()
                {
                    Id = "SystemTime 0x3FEF",
                    Value = DateTime.UtcNow.AddMinutes(1).ToString("o")
                }
            }
            };

            await graphClient.Me.Messages[id].Reply(email, comment)
            .Request()
            .PostAsync();

            return SuccessResponse();

        }

        [MethodDescription("Teams", "Searches the chat logs based on the provided member and chat type.")]
        public async Task<IEnumerable<Models.Microsoft.TeamsChat>> SearchChat(
            [ParameterDescription("The chat member to filter on.")] string member = null,
            [ParameterDescription("The type of chat to filter on.")] ChatType? chatType = null)
        {
            var graphClient = GetAuthenticatedClient();
            var filterQuery = string.Empty;

            if (!string.IsNullOrEmpty(member))
            {
                filterQuery += $"members/any(m: contains(tolower(m/displayName), '{member.ToLower()}'))";
            }

            if (chatType.HasValue)
            {
                filterQuery += (string.IsNullOrEmpty(filterQuery) ? "" : " and ") + $"chatType eq '{chatType.Value}'";
            }

            var request = graphClient.Me.Chats.Request();

            if (!string.IsNullOrEmpty(filterQuery))
            {
                request = request.Filter(filterQuery);
            }

            var items = await request.Expand("members").Select("id,webUrl").GetAsync();

            return items.Select(_mapper.Map<Models.Microsoft.TeamsChat>);
        }

        [MethodDescription("Me", "Changes the password of the current user.")]
        public async Task<string> ChangeMyPassword(
            [ParameterDescription("The new password.")] string newPassword,
            [ParameterDescription("The current password.")] string currentPassword)
        {
            var graphClient = GetAuthenticatedClient();

            await graphClient.Me.ChangePassword(currentPassword, newPassword)
                .Request()
                .PostAsync();

            return newPassword;
        }

        // Get information about the user.
        [MethodDescription("Me", "Retrieves the profile of the current user.")]
        public async Task<string> MyProfile()
        {
            var graphClient = GetAuthenticatedClient();
            var me = await graphClient.Me.Request().GetAsync();
            return JsonConvert.SerializeObject(_mapper.Map<Models.Microsoft.User>(me));
        }

        // gets information about the user's manager.
        [MethodDescription("Me", "Retrieves information about the current user's manager.")]
        public async Task<Models.Microsoft.User> MyManager()
        {
            var graphClient = GetAuthenticatedClient();
            var manager = await graphClient.Me.Manager.Request().GetAsync() as User;
            return _mapper.Map<Models.Microsoft.User>(manager);
        }

        [MethodDescription("Mail", "Gets mail for the user using the Microsoft Graph API")]
        public async Task<string> SearchMail(
            [ParameterDescription("Subject of the email to search for")] string subject = null,
            [ParameterDescription("Sender of the email to search for")] string from = null,
            [ParameterDescription("Start date in ISO 8601 format.")] string fromDate = null,
            [ParameterDescription("End date in ISO 8601 format")] string toDate = null)
        {
            var graphClient = GetAuthenticatedClient();

            var filterQueries = new List<string>();

            if (!string.IsNullOrEmpty(subject))
            {
                filterQueries.Add($"contains(subject, '{subject}')");
            }

            if (!string.IsNullOrEmpty(from))
            {
                filterQueries.Add($"from/emailAddress/address eq '{from}'");
            }

            if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out DateTime parsedFromDate))
            {
                filterQueries.Add($"receivedDateTime ge {parsedFromDate.ToString("s") + "Z"}");
            }

            if (!string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out DateTime parsedToDate))
            {
                filterQueries.Add($"receivedDateTime le {parsedToDate.ToString("s") + "Z"}");
            }

            var filterQuery = string.Join(" and ", filterQueries);
            var selectQuery = "id,webLink,bodyPreview,subject,receivedDateTime";

            var messages = await graphClient.Me.Messages
                .Request()
                .Filter(filterQuery)
                .Select(selectQuery)
                .GetAsync();

   return JsonConvert.SerializeObject(messages
                .Take(10)
                .Select(_mapper.Map<Models.Microsoft.Email>));

   /*         return messages
                .Take(10)
                .Select(_mapper.Map<Models.Graph.Email>);*/
        }

        // Search for teams based on team name or description.
        [MethodDescription("Teams", "Searches for your teams based on name or description.")]
        public async Task<IEnumerable<Models.Microsoft.Team>> SearchTeams(
            [ParameterDescription("The team name to filter on.")] string name = null,
            [ParameterDescription("The description to filter on.")] string description = null)
        {
            var graphClient = GetAuthenticatedClient();

            var groups = await graphClient.Me.JoinedTeams
                                .Request()
                                .GetAsync();

            var filteredGroups = groups.Where(group =>
                (string.IsNullOrEmpty(name) || group.DisplayName.ToLower().Contains(name.ToLower())) &&
                (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(group.Description) || group.Description.ToLower().Contains(description.ToLower()))
            );

            return filteredGroups.Select(t => _mapper.Map<Models.Microsoft.Team>(t));
        }
    }
}