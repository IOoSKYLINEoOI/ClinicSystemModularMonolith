using CSharpFunctionalExtensions;
using Patients.Core.Enums;
using Patients.Core.Models;

namespace Patients.Core.Interfaces.Services;

public interface IPatientService
{
    Task<Result> AddPatient(Guid userId);

    Task<Result> UpdatePatient(
        Guid id,
        BloodGroup type,
        RhFactor rh,
        KellFactor kell);

    Task<Result<Patient>> GetPatient(Guid id);
}