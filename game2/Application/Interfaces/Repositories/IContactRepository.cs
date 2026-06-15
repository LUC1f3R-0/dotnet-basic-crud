using TestCrudApplication.Domain.Entities;

namespace TestCrudApplication.Application.Interfaces;

public interface IContactRepository
{
    Task<List<Contact>> GetAllAsync();

    Task<Contact?> GetByUuidAsync(Guid uuid);

    Task<Contact> CreateAsync(Contact contact);

    Task<bool> UpdateAsync(Contact contact);

    Task<bool> DeleteAsync(Contact contact);
}