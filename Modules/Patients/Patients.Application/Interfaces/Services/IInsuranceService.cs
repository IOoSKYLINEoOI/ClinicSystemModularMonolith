using CSharpFunctionalExtensions;
using Patients.Core.Models;

namespace Patients.Application.Interfaces.Services;

public interface IInsuranceService
{
    Task<Result> AddInsurance(
        Guid patientId,
        string insuranceCompany,
        string policyNumber,
        DateOnly validFrom,
        DateOnly validTo);

    Task<Result> UpdateIsurance(
        Guid id,
        string insuranceCompany,
        string policyNumber,
        DateOnly validFrom,
        DateOnly validTo);

    Task<Result<Insurance>> GetInsuranceById(Guid id);
    Task<Result<List<Insurance>>> GetAllInsurancesByPatientId(Guid patientId);
}