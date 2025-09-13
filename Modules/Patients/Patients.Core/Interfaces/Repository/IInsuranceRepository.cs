using Patients.Core.Models;

namespace Patients.Core.Interfaces.Repository;

public interface IInsuranceRepository
{
    Task Add(Insurance insurance);
    Task Update(Insurance insurance);
    Task Delete(Guid id);
    Task<Insurance?> GetById(Guid id);
    Task<List<Insurance>> GetByAllPatientId(Guid patientId);
}