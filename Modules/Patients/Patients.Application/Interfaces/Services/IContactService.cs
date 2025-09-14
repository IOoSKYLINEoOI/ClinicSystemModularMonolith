using CSharpFunctionalExtensions;
using Patients.Core.Models;

namespace Patients.Application.Interfaces.Services;

public interface IContactService
{
    Task<Result> AddContact(
        Guid patientId,
        string contactName,
        string relationship,
        string phone,
        string? email);

    Task<Result> UpdateContact(
        Guid id,
        string contactName,
        string relationship,
        string phone,
        string? email);

    Task<Result<Contact>> GetContactById(Guid id);
    Task<Result<List<Contact>>> GetAllContactsByPatientId(Guid patientId);
}