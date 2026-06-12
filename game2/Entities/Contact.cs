namespace BeginnerCrud.Domain.Entities;

public class Contact
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public string Email { get; set; } = string.Empty;

    public int Age { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
} 