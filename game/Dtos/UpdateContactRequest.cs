using System.ComponentModel.DataAnnotations;

namespace BeginnerCrud.Api.Dtos;

public class UpdateContactRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(1, 150)]
    public int Age { get; set; }
}