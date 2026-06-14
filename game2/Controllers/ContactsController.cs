using Microsoft.AspNetCore.Mvc;
using TestCrudApplication.Api.Dtos;

namespace TestCrudApplication.Api.Controllers;

[ApiController]
[Route("Contacts")]
public class ContactsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok("this is working");
    }

    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetByUuid(Guid guid)
    {
        return Ok(guid);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact(CreateContactRequest request)
    {
        return Ok(request);
    }


    [HttpPut("{uuid:guid}")]
    public async Task<IActionResult> UpdataContact(UpdateContactRequest request, Guid guid)
    {
        return Ok(new { request, guid });
    }

    [HttpDelete("{uuid:guid}")]
    public async Task<IActionResult> DeleteContact(Guid guid)
    {
        return Ok(guid);
    }
}