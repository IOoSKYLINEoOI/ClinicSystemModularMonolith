using CSharpFunctionalExtensions;
using Patients.Core.Interfaces.Repository;
using Patients.Core.Interfaces.Services;
using Patients.Core.Models;

namespace Patients.Application.Services;

public class InsuranceService : IInsuranceService
{
    private readonly IInsuranceRepository _insuranceRepository;

    public InsuranceService(IInsuranceRepository insuranceRepository)
    {
        _insuranceRepository = insuranceRepository;
    }

    public async Task<Result> AddInsurance(
        Guid patientId,
        string insuranceCompany,
        string policyNumber,
        DateOnly validFrom,
        DateOnly validTo)
    {
        var insuranceResult = Insurance.Create(
            id: Guid.NewGuid(),
            patientId: patientId,
            insuranceCompany: insuranceCompany,
            policyNumber: policyNumber,
            validFrom: validFrom,
            validTo: validTo);

        if (insuranceResult.IsFailure)
            return Result.Failure(insuranceResult.Error);

        await _insuranceRepository.Add(insuranceResult.Value);
        return Result.Success();
    }

    public async Task<Result> UpdateIsurance(
        Guid id,
        string insuranceCompany,
        string policyNumber,
        DateOnly validFrom,
        DateOnly validTo)
    {
        var insurance = await _insuranceRepository.GetById(id);
        if (insurance == null)
            return Result.Failure($"Insurance with ID {id} not found");

        var insuranceResult = Insurance.Create(
            id: insurance.Id,
            patientId: insurance.PatientId,
            insuranceCompany: insuranceCompany,
            policyNumber: policyNumber,
            validFrom: validFrom,
            validTo: validTo);

        if (insuranceResult.IsFailure)
            return Result.Failure(insuranceResult.Error);

        await _insuranceRepository.Update(insuranceResult.Value);
        return Result.Success();
    }

    public async Task<Result<Insurance>> GetInsuranceById(Guid id)
    {
        var insurance = await _insuranceRepository.GetById(id);
        if (insurance == null)
            return Result.Failure<Insurance>($"Insurance with ID {id} not found");
        
        return Result.Success(insurance);
    }

    public async Task<Result<List<Insurance>>> GetAllInsurancesByPatientId(Guid patientId)
    {
        return await _insuranceRepository.GetByAllPatientId(patientId);
    }
}