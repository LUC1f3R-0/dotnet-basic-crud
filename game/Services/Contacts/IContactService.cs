using BeginnerCrud.Api.Dtos;

namespace BeginnerCrud.Api.Services.Contacts;

public interface IContactService
{
    Task<List<ContactResponse>> GetAllAsync();

    Task<ContactResponse?> GetByUuidAsync(Guid uuid);

    Task<ContactResponse> CreateAsync(CreateContactRequest request);

    Task<bool> UpdateAsync(Guid uuid, UpdateContactRequest request);

    Task<bool> DeleteAsync(Guid uuid);
}