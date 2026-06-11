using BeginnerCrud.Api.Data;
using BeginnerCrud.Application.Repositories.Contacts;
using BeginnerCrud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeginnerCrud.Infrastructure.Repositories.Contacts;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _dbContext;

    public ContactRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        return await _dbContext.Contacts
            .OrderByDescending(contact => contact.CreatedAt)
            .ToListAsync();
    }

    public async Task<Contact?> GetByUuidAsync(Guid uuid)
    {
        return await _dbContext.Contacts
            .FirstOrDefaultAsync(contact => contact.Uuid == uuid);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Contacts
            .AnyAsync(contact => contact.Email == email);
    }

    public async Task<bool> ExistsByEmailForAnotherContactAsync(string email, Guid uuid)
    {
        return await _dbContext.Contacts
            .AnyAsync(contact =>
                contact.Email == email &&
                contact.Uuid != uuid
            );
    }

    public async Task AddAsync(Contact contact)
    {
        await _dbContext.Contacts.AddAsync(contact);
    }

    public void Delete(Contact contact)
    {
        _dbContext.Contacts.Remove(contact);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}