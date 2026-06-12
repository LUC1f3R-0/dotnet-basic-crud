using System.ComponentModel.DataAnnotations;

namespace TestCrudApplication.Api.Dtos;

public class CreateContactRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Range(1, 150)]
    public int Age { get; set; }
}