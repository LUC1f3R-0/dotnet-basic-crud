using TestCrudApplication.Application.Interfaces;
using TestCrudApplication.Domain.Entities;
using TestCrudApplication.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace TestCrudApplication.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _dbContext;

    public ContactRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        return await _dbContext.Contacts.ToListAsync();
    }

    public async Task<Contact?> GetByUuidAsync(Guid uuid)
    {
        return await _dbContext.Contacts
        .FirstOrDefaultAsync(x => x.Uuid == uuid);
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        await _dbContext.Contacts.AddAsync(contact);
        await _dbContext.SaveChangesAsync();

        return contact;
    }

    public async Task<bool> UpdateAsync(Contact contact)
    {
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Contacts.AnyAsync(contact => contact.Email == email);
    }

    public async Task<bool> DeleteAsync(Contact contact)
    {
        _dbContext.Contacts.Remove(contact);

        await _dbContext.SaveChangesAsync();
        return true;
    }
    
}