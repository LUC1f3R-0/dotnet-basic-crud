using TestCrudApplication.Application.Dtos.Contacts.Responses;

namespace TestCrudApplication.Application.Interfaces;

public interface IContactService
{
    Task<List<ContactResponse>> GetAllAsync();

    Task<ContactResponse?> GetByUuidAsync(Guid uuid);

    Task<ContactResponse> CreateAsync(CreateContactRequest request);

    Task<ContactResponse?> UpdateByUuidAsync(Guid uuid, UpdateContactRequest request);

    Task<ContactResponse?> DeleteByUuidAsync(Guid uuid);
}