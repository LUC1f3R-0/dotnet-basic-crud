using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestCrudApplication.Application.Dtos.Contacts.Responses;

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed class UpdateContactRequest
{
    [Range(1, 150, ErrorMessage = "Age must be between 1 and 150.")]
    public int Age { set; get; }
}