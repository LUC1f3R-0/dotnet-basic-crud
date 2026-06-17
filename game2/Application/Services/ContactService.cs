using TestCrudApplication.Application.Interfaces;
using TestCrudApplication.Application.Dtos.Contacts.Responses;
using TestCrudApplication.Infrastructure.Repositories;
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
        return null;
    }

    public async Task<ContactResponse> CreateAsync(CreateContactRequest request)
    {
        var now = DateTime.UtcNow;

        var contact = new Contact
        {
            Uuid = Guid.NewGuid(),
            Email = request.Email.Trim(),
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

        var isUpdated = await _contactRepository.UpdateAsync(contact);

        if (!isUpdated)
        {
            return null;
        }

        return MapToResponse(contact);
    }

    public async Task<bool> DeleteAsync(Guid uuid)
    {
        return true;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var normalizeEmail = email.Trim().ToLowerInvariant();
        return await _contactRepository.ExistsByEmailAsync(normalizeEmail);
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