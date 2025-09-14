using CSharpFunctionalExtensions;
using Patients.Application.Commands.Patient;
using Patients.Core.Models;

namespace Patients.Application.Interfaces.Services;

public interface IPatientService
{
    Task<Result> AddPatient(Guid userId);

    Task<Result> UpdatePatient(UpdatePatientCommand updatePatientCommand);

    Task<Result<Patient>> GetPatient(Guid id);
}