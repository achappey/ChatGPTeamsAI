﻿using ChatGPTeamsAI.Data.Attributes;
using ChatGPTeamsAI.Data.Extensions;
using ChatGPTeamsAI.Data.Models;
using ChatGPTeamsAI.Data.Models.Simplicate;

namespace ChatGPTeamsAI.Data.Clients.Simplicate
{
    internal partial class SimplicateFunctionsClient
    {

        [MethodDescription("CRM", "Adds a new organization to Simplicate.")]
        public async Task<NoOutputResponse> AddNewOrganization(
            [ParameterDescription("The name of the organization.")] string name,
            [ParameterDescription("The email of the organization.")] string? email = null,
            [ParameterDescription("The linkedin url of the organization.")] string? linkedin = null,
            [ParameterDescription("The website url of the organization.")] string? website = null,
            [ParameterDescription("The industry id of the organization.")] string? industryId = null,
            [ParameterDescription("A note to add to the organization.")] string? note = null,
            [ParameterDescription("The phone number of the organization.")] string? phone = null,
            [ParameterDescription("The person id to be linked to the organization.")] string? personId = null)
        {
            var linkedPersons = new List<object>();

            if (!string.IsNullOrEmpty(personId))
            {
                linkedPersons.Add(new
                {
                    person_id = personId
                });
            }

            var orgData = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(name)) orgData["name"] = name;
            if (!string.IsNullOrEmpty(email)) orgData["email"] = email;
            if (!string.IsNullOrEmpty(website)) orgData["url"] = website;
            if (!string.IsNullOrEmpty(linkedin)) orgData["linkedin_url"] = linkedin;
            if (!string.IsNullOrEmpty(phone)) orgData["phone"] = phone;
            if (!string.IsNullOrEmpty(note)) orgData["note"] = note;
            if (!string.IsNullOrEmpty(industryId)) orgData["industry"] = new { id = industryId };
            if (linkedPersons.Any()) orgData["linked_persons_contacts"] = linkedPersons;

            var response = await _httpClient.PostAsync("crm/organization", orgData.PrepareJsonContent());

            if (response.IsSuccessStatusCode)
            {
                return SuccessResponse();
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("CRM", "Adds a new person to Simplicate.")]
        public async Task<SimplicateResponseBase<NoOutputResponse>> AddNewPerson(
            [ParameterDescription("The family name of the person.")] string? familyName,
            [ParameterDescription("The full name of the person.")] string? fullName,
            [ParameterDescription("The first name of the person.")] string? firstName = null,
            [ParameterDescription("The job title of the person.")] string? jobTitle = null,
            [ParameterDescription("The email of the person.")] string? email = null,
            [ParameterDescription("The mobile phone number of the person.")] string? mobilePhone = null,
            [ParameterDescription("The work phone number of the person.")] string? workPhone = null,
            [ParameterDescription("A note to add to the person.")] string? note = null,
            [ParameterDescription("The organization id to be linked to the person.")] string? organizationId = null)
        {
            var linkedOrganizations = new List<object>();

            if (!string.IsNullOrEmpty(organizationId))
            {
                var linkedOrganizationData = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(organizationId)) linkedOrganizationData["organization_id"] = organizationId;
                if (!string.IsNullOrEmpty(workPhone)) linkedOrganizationData["work_mobile"] = workPhone;
                if (!string.IsNullOrEmpty(jobTitle)) linkedOrganizationData["work_function"] = jobTitle;
                if (!string.IsNullOrEmpty(email)) linkedOrganizationData["work_email"] = email;

                if (linkedOrganizationData.Any())
                {
                    linkedOrganizations.Add(linkedOrganizationData);
                }
            }

            var personData = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(firstName)) personData["first_name"] = firstName;
            if (!string.IsNullOrEmpty(familyName)) personData["family_name"] = familyName;
            if (!string.IsNullOrEmpty(fullName)) personData["full_name"] = fullName;
            if (!string.IsNullOrEmpty(email)) personData["email"] = email;
            if (linkedOrganizations.Any()) personData["linked_as_contact_to_organization"] = linkedOrganizations;
            if (!string.IsNullOrEmpty(mobilePhone)) personData["phone"] = mobilePhone;
            if (!string.IsNullOrEmpty(note)) personData["note"] = note;

            var response = await _httpClient.PostAsync("crm/person", personData.PrepareJsonContent());

            if (response.IsSuccessStatusCode)
            {
                return new SimplicateResponseBase<NoOutputResponse>()
                {
                    Data = SuccessResponse()
                };
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("CRM", "Search for persons", "ExportPersons")]
        public async Task<ChatGPTeamsAIClientResponse?>? SearchPersons(
            [ParameterDescription("The first name of the person.")] string? firstName = null,
            [ParameterDescription("The family name of the person.")] string? familyName = null,
            [ParameterDescription("The email of the person.")] string? email = null,
            [ParameterDescription("The name of the relation manager of the person.")] string? relationManager = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("The phone number of the person.")] string? phone = null,
            [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreatePersonFilters(firstName, familyName, email, relationManager, createdAfter, phone);
            var result = await FetchSimplicateDataCollection<Person>(filters, "crm/person", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreatePersonFilters(
            string? firstName,
            string? familyName,
            string? email,
            string? relationManager,
            string? createdAfter,
            string? phone)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(firstName)) filters["[first_name]"] = $"*{firstName}*";
            if (!string.IsNullOrEmpty(familyName)) filters["[family_name]"] = $"*{familyName}*";
            if (!string.IsNullOrEmpty(email)) filters["[email]"] = $"*{email}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(phone)) filters["[phone]"] = $"*{phone}*";
            if (!string.IsNullOrEmpty(relationManager)) filters["[relation_manager.name]"] = $"*{relationManager}*";

            return filters;
        }

        [MethodDescription("Export", "Exports a list of persons")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportPersons(
            [ParameterDescription("The first name of the person.")] string? firstName = null,
            [ParameterDescription("The family name of the person.")] string? familyName = null,
            [ParameterDescription("The email of the person.")] string? email = null,
            [ParameterDescription("The name of the relation manager of the person.")] string? relationManager = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("The phone number of the person.")] string? phone = null)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreatePersonFilters(firstName, familyName, email, relationManager, createdAfter, phone);
            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<Person>($"crm/person?{queryString}");

            var result = new SimplicateDataCollectionResponse<Person>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of organizations")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportOrganizations(
            [ParameterDescription("The name of the organization.")] string? name = null,
            [ParameterDescription("The email of the organization.")] string? email = null,
            [ParameterDescription("The phone number of the organization.")] string? phone = null,
            [ParameterDescription("The industry of the organization.")] string? industry = null,
            [ParameterDescription("The relation type of the organization.")] string? relationType = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("The relation manager of the organization.")] string? relationManager = null)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreateOrganizationFilters(name, email, phone, industry, relationType, createdAfter, relationManager);
            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<Organization>($"crm/organization?{queryString}");

            var result = new SimplicateDataCollectionResponse<Organization>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }


        [MethodDescription("CRM", "Search for organizations", "ExportOrganizations")]
        public async Task<ChatGPTeamsAIClientResponse?>? SearchOrganizations(
            [ParameterDescription("The name of the organization.")] string? name = null,
            [ParameterDescription("The email of the organization.")] string? email = null,
            [ParameterDescription("The phone number of the organization.")] string? phone = null,
            [ParameterDescription("The industry of the organization.")] string? industry = null,
            [ParameterDescription("The relation type of the organization.")] string? relationType = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("The relation manager of the organization.")] string? relationManager = null,
            [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreateOrganizationFilters(name, email, phone, industry, relationType, createdAfter, relationManager);
            var result = await FetchSimplicateDataCollection<Organization>(filters, "crm/organization", pageNumber);

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreateOrganizationFilters(
                string? name,
                string? email,
                string? phone,
                string? industry,
                string? relationType,
                string? createdAfter,
                string? relationManager)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(name)) filters["[name]"] = $"*{name}*";
            if (!string.IsNullOrEmpty(email)) filters["[email]"] = $"*{email}*";
            if (!string.IsNullOrEmpty(phone)) filters["[phone]"] = $"*{phone}*";
            if (!string.IsNullOrEmpty(industry)) filters["[industry.name]"] = $"*{industry}*";
            if (!string.IsNullOrEmpty(relationType)) filters["[relation_type.label]"] = $"*{relationType}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(relationManager)) filters["[relation_manager.name]"] = $"*{relationManager}*";

            return filters;
        }

        [MethodDescription("CRM", "Search for contact persons", "ExportContactPersons")]
        public async Task<ChatGPTeamsAIClientResponse?>? SearchContactPersons(
                    [ParameterDescription("The full name of the contact person.")] string? fullName = null,
                    [ParameterDescription("The organization name of the contact person.")] string? organizationName = null,
                    [ParameterDescription("The work email of the contact person.")] string? workEmail = null,
                    [ParameterDescription("The work function of the contact person.")] string? workFunction = null,
                    [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
                    [ParameterDescription("The work phone of the contact person.")] string? workPhone = null,
                    [ParameterDescription("The work mobile of the contact person.")] string? workMobile = null,
                    [ParameterDescription("The page number.")] long pageNumber = 1)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreateContactPersonsFilters(fullName, organizationName, workEmail, workFunction, createdAfter, workPhone, workMobile);
            var result = await FetchSimplicateDataCollection<ContactPerson>(filters, "crm/contactperson", pageNumber);
            return ToChatGPTeamsAIResponse(result);
        }

        [MethodDescription("Export", "Exports a list of contact persons")]
        public async Task<ChatGPTeamsAIClientResponse?> ExportContactPersons(
            [ParameterDescription("The full name of the contact person.")] string? fullName = null,
            [ParameterDescription("The organization name of the contact person.")] string? organizationName = null,
            [ParameterDescription("The work email of the contact person.")] string? workEmail = null,
            [ParameterDescription("The work function of the contact person.")] string? workFunction = null,
            [ParameterDescription("Created at or after this date and time (format: yyyy-MM-dd HH:mm:ss).")] string? createdAfter = null,
            [ParameterDescription("The work phone of the contact person.")] string? workPhone = null,
            [ParameterDescription("The work mobile of the contact person.")] string? workMobile = null)
        {
            createdAfter?.EnsureValidDateFormat();

            var filters = CreateContactPersonsFilters(fullName, organizationName, workEmail, workFunction, createdAfter, workPhone, workMobile);
            var queryString = BuildQueryString(filters);
            var response = await _httpClient.PagedRequest<ContactPerson>($"crm/contactperson?{queryString}");

            var result = new SimplicateDataCollectionResponse<ContactPerson>()
            {
                Data = response
            };

            return ToChatGPTeamsAIResponse(result);
        }

        private Dictionary<string, string> CreateContactPersonsFilters(
            string? fullName,
            string? organizationName,
            string? workEmail,
            string? workFunction,
            string? createdAfter,
            string? workPhone,
            string? workMobile)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(fullName)) filters["[person.full_name]"] = $"*{fullName}*";
            if (!string.IsNullOrEmpty(organizationName)) filters["[organization.name]"] = $"*{organizationName}*";
            if (!string.IsNullOrEmpty(workEmail)) filters["[work_email]"] = $"*{workEmail}*";
            if (!string.IsNullOrEmpty(workFunction)) filters["[work_function]"] = $"*{workFunction}*";
            if (!string.IsNullOrEmpty(createdAfter)) filters["[created_at][ge]"] = createdAfter;
            if (!string.IsNullOrEmpty(workPhone)) filters["[work_phone]"] = $"*{workPhone}*";
            if (!string.IsNullOrEmpty(workMobile)) filters["[work_mobile]"] = $"*{workMobile}*";

            return filters;
        }

        [MethodDescription("CRM", "Gets all my organization profiles")]
        public async Task<ChatGPTeamsAIClientResponse?>? GetAllMyOrganizations()
        {
            var response = await _httpClient.GetAsync("crm/myorganizationprofile");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.FromJson<SimplicateDataCollectionResponse<MyOrganizationProfile>?>();

                return ToChatGPTeamsAIResponse(result);
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("CRM", "Gets all relation types")]
        public async Task<ChatGPTeamsAIClientResponse?>? GetAllRelationTypes()
        {
            var response = await _httpClient.GetAsync("crm/relationtype");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.FromJson<SimplicateDataCollectionResponse<RelationType>?>();

                return ToChatGPTeamsAIResponse(result);
            }

            throw new Exception(response.ReasonPhrase);
        }

        [MethodDescription("CRM", "Gets all industry types")]
        public async Task<ChatGPTeamsAIClientResponse?>? GetAllIndustries()
        {
            var response = await _httpClient.GetAsync("crm/industry");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.FromJson<SimplicateDataCollectionResponse<Industry>?>();

                return ToChatGPTeamsAIResponse(result);
            }

            throw new Exception(response.ReasonPhrase);
        }
    }
}