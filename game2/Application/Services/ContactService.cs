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
        return new ContactResponse
        {
            Uuid = Guid.NewGuid(),
            Email = request.Email,
            Age = request.Age,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public async Task<bool> UpdateAsync(Guid uuid, UpdateContactRequest request)
    {
        return true;
    }

    public async Task<bool> DeleteAsync(Guid uuid)
    {
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