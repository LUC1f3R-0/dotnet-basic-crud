using Microsoft.AspNetCore.Mvc;
// using Org.BouncyCastle.Asn1.X509;
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
        var response = ApiResponse<List<ContactResponse>>.Ok("Contact revceived successfully",contacts);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetContact(Guid guid)
    {
        var contact = await _contactService.GetByUuidAsync(guid);
        if (contact is null)
        {
            var notFoundResponse = ApiResponse<ContactResponse>.Fail("Contact not found");
            return NotFound(notFoundResponse);
        }
        var response = ApiResponse<ContactResponse>.Ok("data recieved successfully", contact);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact(CreateContactRequest request)
    {
        try
        {
            var contact = await _contactService.CreateAsync(request);
            var response = ApiResponse<ContactResponse>.Ok("Contact created successfully",contact);
            return StatusCode(StatusCodes.Status201Created,response);
        }
        catch (InvalidOperationException exception)
        {
            var conflictResponse = ApiResponse<ContactResponse>.Fail(exception.Message);
            return Conflict(conflictResponse);
        }
    }

    [HttpPut("{guid:guid}")]
    public async Task<IActionResult> UpdateContact(Guid guid, UpdateContactRequest request)
    {
        var contact = await _contactService.UpdateByUuidAsync(guid, request);
        if (contact is null)
        {
            var notFoundResponse = ApiResponse<ContactResponse>.Fail("Contact not found");
            return NotFound(notFoundResponse);
        }
        var response = ApiResponse<ContactResponse>.Ok("Contact updated successfully", contact);
        return StatusCode(StatusCodes.Status201Created,response);
    }

    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> DeleteByUuid(Guid guid)
    {
        var contact = await _contactService.DeleteByUuidAsync(guid);
        if (contact is null)
        {
            var notFoundResponse = ApiResponse<ContactResponse>.Fail("Contact not found");
            return NotFound(notFoundResponse);
        }
        var response = ApiResponse<ContactResponse>.Ok("Deleted successfully", contact);
        return Ok(response);
    }
}