using Microsoft.AspNetCore.Mvc;
using TestCrudApplication.Application.Dtos.Contacts.Responses;
using TestCrudApplication.Application.Interfaces;
using TestCrudApplication.Shared.Dtos;

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
        var contact = await _contactService.CreateAsync(request);
        
        var response = ApiResponse<ContactResponse>.Ok
        (
            "Contact created successfully",
            contact
        );
        
        return StatusCode(StatusCodes.Status201Created,response);
    }

    [HttpPut("{guid:guid}")]
    public async Task<IActionResult> UpdateContact(Guid guid, UpdateContactRequest request)
    {
        return Ok(await _contactService.UpdateAsync(guid, request));
    }

    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> DeleteContact(Guid guid)
    {
        return Ok(await _contactService.DeleteAsync(guid));
    }
}