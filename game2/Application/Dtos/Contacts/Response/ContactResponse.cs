namespace TestCrudApplication.Application.Dtos.Contacts.Responses;

public class ContactResponse
{
    public Guid Uuid { get; set; }

    public string Email { get; set; } = string.Empty;

    public int Age { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}