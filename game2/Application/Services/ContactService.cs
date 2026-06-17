using TestCrudApplication.Application.Interfaces;
using TestCrudApplication.Application.Dtos.Contacts.Responses;
using TestCrudApplication.Domain.Entities;

namespace TestCrudApplication.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<List<ContactResponse>> GetAllAsync()
    {
        var contacts = await _contactRepository.GetAllAsync();

        return contacts
        .Select(MapToResponse)
        .ToList();
    }

    public async Task<ContactResponse?> GetByUuidAsync(Guid uuid)
    {
        var existContact = await _contactRepository.GetByUuidAsync(uuid);
        if (existContact is null)
        {
            return null;
        }
        return MapToResponse(existContact);
    }

    public async Task<ContactResponse> CreateAsync(CreateContactRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        
        var emailExists = await _contactRepository.ExistsByEmailAsync(normalizedEmail);
        if (emailExists)
        {
            throw new InvalidOperationException("A contact with this email already exists.");
        }
        var now = DateTime.UtcNow;
        var contact = new Contact
        {
            Uuid = Guid.NewGuid(),
            Email = normalizedEmail,
            Age = request.Age,
            CreatedAt = now,
            UpdatedAt = now
        };
        var createdContact = await _contactRepository.CreateAsync(contact);
        return MapToResponse(createdContact);
    }

    public async Task<ContactResponse?> UpdateByUuidAsync(Guid uuid, UpdateContactRequest request)
    {
        var contact = await _contactRepository.GetByUuidAsync(uuid);
        if (contact is null)
        {
            return null;
        }
        contact.Age = request.Age;
        contact.UpdatedAt = DateTime.UtcNow;
        
        await _contactRepository.UpdateAsync(contact);
        return MapToResponse(contact);
    }

    public async Task<ContactResponse?> DeleteByUuidAsync(Guid uuid)
    {
        var existUuid = await _contactRepository.GetByUuidAsync(uuid);
        if (existUuid is null)
        {
            return null;
        }
        var responce = await _contactRepository.DeleteByUuidAsync(existUuid);
        return MapToResponse(responce);
    }
    
    private static ContactResponse MapToResponse(Contact contact)
    {
        return new ContactResponse
        {
            Uuid = contact.Uuid,
            Email = contact.Email,
            Age = contact.Age,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }    
}