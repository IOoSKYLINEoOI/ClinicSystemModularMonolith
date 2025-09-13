using Patients.Core.Models;

namespace Patients.Core.Interfaces.Repository;

public interface IContactRepository
{
    Task Add(Contact contact);
    Task Update(Contact contact);
    Task Delete(Guid id);
    Task<Contact?> GetById(Guid id);
    Task<List<Contact>> GetByAllPatientId(Guid patientId);
}