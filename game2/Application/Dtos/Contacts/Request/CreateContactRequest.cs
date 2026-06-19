using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestCrudApplication.Application.Dtos.Contacts.Responses;

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed class CreateContactRequest
{
    [Required(ErrorMessage = "Email is requried")]
    [EmailAddress(ErrorMessage = "Email must be valid.")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Range(1, 150, ErrorMessage = "Age must be between 1 and 150.")]
    public int Age { get; set; }
}