using CSharpFunctionalExtensions;
using Patients.Core.Interfaces.Repository;
using Patients.Core.Interfaces.Services;
using Patients.Core.Models;

namespace Patients.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<Result> AddContact(
        Guid patientId,
        string contactName,
        string relationship,
        string phone,
        string? email)
    {
        var contactResult = Contact.Create(
            id: Guid.NewGuid(), 
            patientId: patientId,
            contactName: contactName,
            relationship: relationship,
            phone: phone,
            email: email);
        
        if(contactResult.IsFailure)
            return Result.Failure(contactResult.Error);
        
        await _contactRepository.Add(contactResult.Value);
        return Result.Success();
    }

    public async Task<Result> UpdateContact(
        Guid id,
        string contactName,
        string relationship,
        string phone,
        string? email)
    {
        var contact = await _contactRepository.GetById(id);
        if(contact == null)
            return Result.Failure($"Contact with ID {id} not found");

        var contactResult = Contact.Create(
            id: contact.Id,
            patientId: contact.PatientId,
            contactName: contactName,
            relationship: relationship,
            phone: phone,
            email: email);
        
        if(contactResult.IsFailure)
            return Result.Failure(contactResult.Error);
        
        await _contactRepository.Update(contactResult.Value);
        return Result.Success();
    }

    public async Task<Result<Contact>> GetContactById(Guid id)
    {
        var contact = await _contactRepository.GetById(id);
        if(contact == null)
            return Result.Failure<Contact>($"Contact with ID {id} not found");
        
        return Result.Success(contact);
    }

    public async Task<Result<List<Contact>>> GetAllContactsByPatientId(Guid patientId)
    {
        return await _contactRepository.GetByAllPatientId(patientId);
    }
}