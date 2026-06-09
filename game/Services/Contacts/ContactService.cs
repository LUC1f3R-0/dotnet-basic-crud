using BeginnerCrud.Api.Data;
using BeginnerCrud.Api.Dtos;
using BeginnerCrud.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeginnerCrud.Api.Services.Contacts;

public class ContactService : IContactService
{
    private readonly AppDbContext _dbContext;

    public ContactService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ContactResponse>> GetAllAsync()
    {
        return await _dbContext.Contacts
        .OrderByDescending(contact => contact.CreatedAt)
        .Select(contact => new ContactResponse
        {
            Uuid = contact.Uuid,
            Email = contact.Email,
            Age = contact.Age,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        })
        .ToListAsync();
    }

    public async Task<ContactResponse?> GetByUuidAsync(Guid uuid)
    {
        var contact = await _dbContext.Contacts
            .FirstOrDefaultAsync(contact => contact.Uuid == uuid);

        if (contact is null)
        {
            return null;
        }

        return new ContactResponse
        {
            Uuid = contact.Uuid,
            Email = contact.Email,
            Age = contact.Age,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }

    public async Task<ContactResponse> CreateAsync(CreateContactRequest request)
    {
        var emailAlreadyExists = await _dbContext.Contacts
            .AnyAsync(contact => contact.Email == request.Email);

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

        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();

        return new ContactResponse
        {
            Uuid = contact.Uuid,
            Email = contact.Email,
            Age = contact.Age,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }

    public async Task<bool> UpdateAsync(Guid uuid, UpdateContactRequest request)
    {
        var contact = await _dbContext.Contacts
            .FirstOrDefaultAsync(contact => contact.Uuid == uuid);

        if (contact is null)
        {
            return false;
        }

        var emailUsedByAnotherContact = await _dbContext.Contacts
            .AnyAsync(existingContact =>
                existingContact.Email == request.Email &&
                existingContact.Uuid != uuid
            );

        if (emailUsedByAnotherContact)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        contact.Email = request.Email;
        contact.Age = request.Age;
        contact.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid uuid)
    {
        var contact = await _dbContext.Contacts
            .FirstOrDefaultAsync(contact => contact.Uuid == uuid);

        if (contact is null)
        {
            return false;
        }

        _dbContext.Contacts.Remove(contact);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}