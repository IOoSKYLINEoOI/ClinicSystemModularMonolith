using Patients.Core.Models;

namespace Patients.Core.Interfaces.Repository;

public interface IPatientRepository
{
    Task Add(Patient patient);
    Task Update(Patient patient);
    Task Delete(Guid id);
    Task<Patient?> GetById(Guid id);
}