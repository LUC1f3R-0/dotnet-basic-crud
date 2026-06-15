using System.ComponentModel.DataAnnotations;

namespace TestCrudApplication.Application.Dtos.Contacts.Responses;

public class UpdateContactRequest
{
    [Range(1, 150)]
    public int Age { set; get; }
}