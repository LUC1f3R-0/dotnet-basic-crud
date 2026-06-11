using BeginnerCrud.Api.Dtos;
using BeginnerCrud.Application.Repositories.Contacts;
using BeginnerCrud.Domain.Entities;

namespace BeginnerCrud.Application.Services.Contacts;

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
        var contact = await _contactRepository.GetByUuidAsync(uuid);

        if (contact is null)
        {
            return null;
        }

        return MapToResponse(contact);
    }

    public async Task<ContactResponse> CreateAsync(CreateContactRequest request)
    {
        var emailAlreadyExists = await _contactRepository
            .ExistsByEmailAsync(request.Email);

        if (emailAlreadyExists)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var contact = new Contact
        {
            Email = request.Email,
            Age = request.Age,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _contactRepository.AddAsync(contact);
        await _contactRepository.SaveChangesAsync();

        return MapToResponse(contact);
    }

    public async Task<bool> UpdateAsync(Guid uuid, UpdateContactRequest request)
    {
        var contact = await _contactRepository.GetByUuidAsync(uuid);

        if (contact is null)
        {
            return false;
        }

        var emailUsedByAnotherContact = await _contactRepository
            .ExistsByEmailForAnotherContactAsync(request.Email, uuid);

        if (emailUsedByAnotherContact)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        contact.Email = request.Email;
        contact.Age = request.Age;
        contact.UpdatedAt = DateTime.UtcNow;

        await _contactRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid uuid)
    {
        var contact = await _contactRepository.GetByUuidAsync(uuid);

        if (contact is null)
        {
            return false;
        }

        _contactRepository.Delete(contact);
        await _contactRepository.SaveChangesAsync();

        return true;
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