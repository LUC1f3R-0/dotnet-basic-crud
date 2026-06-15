using Microsoft.AspNetCore.Mvc;
using TestCrudApplication.Application.Dtos.Contacts.Responses;
using TestCrudApplication.Application.Interfaces;

namespace TestCrudApplication.Api.Controllers;

[ApiController]
[Route("contacts")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contacts = await _contactService.GetAllAsync();
        return Ok(contacts);
    }

    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetByUuid(Guid guid)
    {
        return Ok(await _contactService.GetByUuidAsync(guid));
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact(CreateContactRequest request)
    {
        return Ok(await _contactService.CreateAsync(request));
    }

    [HttpPut("{guid:guid}")]
    public async Task<IActionResult> UpdateContact(
        Guid guid,
        UpdateContactRequest request)
    {
        return Ok(await _contactService.UpdateAsync(guid, request));
    }

    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> DeleteContact(Guid guid)
    {
        return Ok(await _contactService.DeleteAsync(guid));
    }
}