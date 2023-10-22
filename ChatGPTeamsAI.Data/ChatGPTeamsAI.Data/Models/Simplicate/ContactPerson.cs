
using System.Text.Json.Serialization;

namespace ChatGPTeamsAI.Data.Models.Simplicate;

internal class ContactPerson
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("organization")]
    public OrganizationContactPerson? Organization { get; set; }

    [JsonPropertyName("person")]
    public PersonContactPerson? Person { get; set; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("work_function")]
    public string? WorkFunction { get; set; }

    [JsonPropertyName("work_email")]
    public string? WorkEmail { get; set; }

    [JsonPropertyName("work_phone")]
    public string? WorkPhone { get; set; }

    [JsonPropertyName("work_mobile")]
    public string? WorkMobile { get; set; }


}


internal class OrganizationContactPerson
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }
}



internal class PersonContactPerson
{
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }

    [JsonPropertyName("relation_number")]
    public string? RelationNumber { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

}
