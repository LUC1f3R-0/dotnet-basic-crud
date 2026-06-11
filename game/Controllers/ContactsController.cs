using BeginnerCrud.Api.Dtos;
using BeginnerCrud.Application.Services.Contacts;
using BeginnerCrud.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BeginnerCrud.Api.Controllers;

[ApiController]
[Route("api/contacts")]
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

        return Ok(
            ApiResponse<List<ContactResponse>>.Ok(
                "Contacts retrieved successfully.",
                contacts
            )
        );
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetByUuid(Guid uuid)
    {
        var contact = await _contactService.GetByUuidAsync(uuid);

        if (contact is null)
        {
            return NotFound(
                ApiResponse<ContactResponse>.Fail("Contact not found.")
            );
        }

        return Ok(
            ApiResponse<ContactResponse>.Ok(
                "Contact retrieved successfully.",
                contact
            )
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateContactRequest request)
    {
        try
        {
            var createdContact = await _contactService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetByUuid),
                new { uuid = createdContact.Uuid },
                ApiResponse<ContactResponse>.Ok(
                    "Contact created successfully.",
                    createdContact
                )
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                ApiResponse<ContactResponse>.Fail(ex.Message)
            );
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<IActionResult> Update(Guid uuid, UpdateContactRequest request)
    {
        try
        {
            var updated = await _contactService.UpdateAsync(uuid, request);

            if (!updated)
            {
                return NotFound(
                    ApiResponse<object>.Fail("Contact not found.")
                );
            }

            return Ok(
                ApiResponse<object>.Ok(
                    "Contact updated successfully.",
                    null
                )
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                ApiResponse<object>.Fail(ex.Message)
            );
        }
    }

    [HttpDelete("{uuid:guid}")]
    public async Task<IActionResult> Delete(Guid uuid)
    {
        var deleted = await _contactService.DeleteAsync(uuid);

        if (!deleted)
        {
            return NotFound(
                ApiResponse<object>.Fail("Contact not found.")
            );
        }

        return Ok(
            ApiResponse<object>.Ok(
                "Contact deleted successfully.",
                null
            )
        );
    }
}