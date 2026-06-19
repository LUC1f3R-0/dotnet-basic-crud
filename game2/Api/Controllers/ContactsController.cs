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
        var response = ApiResponse<List<ContactResponse>>.Ok("Contacts received successfully.", contacts);
        return Ok(response);
    }

    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetContact(Guid guid)
    {
        var contact = await _contactService.GetByUuidAsync(guid);
        var response = ApiResponse<ContactResponse>.Ok("Contact received successfully.", contact);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactRequest request)
    {
        var contact =await _contactService.CreateAsync(request);
        var response = ApiResponse<ContactResponse>.Ok("Contact created successfully.", contact);
        return CreatedAtAction(nameof(GetContact), new { guid = contact.Uuid }, response);
    }

    [HttpPut("{guid:guid}")]
    public async Task<IActionResult> UpdateContact(Guid guid,[FromBody] UpdateContactRequest request)
    {
        var contact = await _contactService.UpdateByUuidAsync(guid, request);
        var response = ApiResponse<ContactResponse>.Ok("Contact updated successfully.", contact);
        return Ok(response);
    }

    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> DeleteByUuid(Guid guid)
    {
        var contact = await _contactService.DeleteByUuidAsync(guid);
        var response = ApiResponse<ContactResponse>.Ok("Contact deleted successfully.", contact);
        return Ok(response);
    }
}