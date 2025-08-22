using Microsoft.EntityFrameworkCore;
using Patient.Core.Interfaces.Repository;
using Patient.Core.Models;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly PatientDbContext _context;

    public ContactRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task Add(Contact contact)
    {
        var contactEntity = MapToEntity(contact);

        _context.Contacts.Add(contactEntity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Contact contact)
    {
        var contactEntity = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contact.Id);
        if (contactEntity == null)
            throw new InvalidOperationException("Contact not found");

        contactEntity.ContactName = contact.ContactName;
        contactEntity.Phone = contact.Phone;
        contactEntity.Relationship = contact.Relationship;
        contactEntity.Email = contact.Email;
        contactEntity.PatientId = contact.PatientId;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var contactEntity = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        if (contactEntity == null)
            throw new InvalidOperationException("Contact not found");

        _context.Contacts.Remove(contactEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Contact?> GetById(Guid id)
    {
        var contactEntity = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        
        return contactEntity == null ? null : MapToDomain(contactEntity);
    }

    public async Task<List<Contact>> GetByPatientId(Guid patientId)
    {
        var contactEntities = await _context.Contacts
            .AsNoTracking()
            .Where(c => c.PatientId == patientId)
            .ToListAsync();

        return contactEntities.Select(MapToDomain).ToList();
    }

    private static ContactEntity MapToEntity(Contact contact) => new ContactEntity
    {
        Id = contact.Id,
        ContactName = contact.ContactName,
        Relationship = contact.Relationship,
        Phone = contact.Phone,
        Email = contact.Email,
        PatientId = contact.PatientId
    };

    private static Contact MapToDomain(ContactEntity contactEntity) => Contact.FromPersistence(
        contactEntity.Id,
        contactEntity.PatientId,
        contactEntity.ContactName,
        contactEntity.Relationship,
        contactEntity.Phone,
        contactEntity.Email);
}
