using System.ComponentModel.DataAnnotations;

namespace TestCrudApplication.Api.Dtos;

public class UpdateContactRequest
{
    [Range(1, 150)]
    public int Age { set; get; }
}