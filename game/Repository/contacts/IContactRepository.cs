using BeginnerCrud.Domain.Entities;

namespace BeginnerCrud.Application.Repositories.Contacts;

public interface IContactRepository
{
    Task<List<Contact>> GetAllAsync();

    Task<Contact?> GetByUuidAsync(Guid uuid);

    Task<bool> ExistsByEmailAsync(string email);

    Task<bool> ExistsByEmailForAnotherContactAsync(string email, Guid uuid);

    Task AddAsync(Contact contact);

    void Delete(Contact contact);

    Task SaveChangesAsync();
}