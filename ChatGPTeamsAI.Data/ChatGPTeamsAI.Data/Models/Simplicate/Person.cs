

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class Person
{
    
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    public string? Initials { get; set; }

    [JsonPropertyName("linkedin_url")]
    public string? LinkedinUrl { get; set; }

    [JsonPropertyName("simplicate_url")]
    public string? SimplicateUrl { get; set; }

    [JsonPropertyName("relation_manager")]
    public RelationManager? RelationManager { get; set; }

    [JsonPropertyName("linked_as_contact_to_organization")]
    public IEnumerable<LinkedContactPerson>? LinkedAsContactToOrganization { get; set; }
}

internal class LinkedContactPerson
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("work_function")]
    public string? WorkFunction { get; set; }
}