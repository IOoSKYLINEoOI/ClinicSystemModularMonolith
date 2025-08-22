using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Interfaces.Services;
using Employees.Core.Models;
using Employees.Core.Models.Filter;

namespace Employees.Application.Services;

public class LicenseService : ILicenseService
{
    private readonly ILicenseRepository _licenseRepository;

    public LicenseService(ILicenseRepository licenseRepository)
    {
        _licenseRepository = licenseRepository;
    }

    public async Task<Result> AddLicense(
        Guid employeeId,
        string licenseNumber,
        string issuedBy,
        DateOnly issuedAt,
        DateOnly? validUntil)
    {
        var licenseResult = EmployeeLicense.Create(
            id: Guid.NewGuid(),
            employeeId,
            licenseNumber,
            issuedBy,
            issuedAt,
            validUntil);
        
        if(licenseResult.IsFailure)
            return Result.Failure(licenseResult.Error);
        
        await _licenseRepository.Add(licenseResult.Value);
        return Result.Success();
    }

    public async Task<Result> UpdateLicense(
        Guid id,
        string licenseNumber,
        string issuedBy,
        DateOnly? validUntil)
    {
        var licenseResult = await _licenseRepository.GetById(id);
        if(licenseResult.IsFailure)
            return Result.Failure(licenseResult.Error);

        var licenseUpdateResult = EmployeeLicense.Create(
            id,
            licenseResult.Value.EmployeeId,
            licenseNumber,
            issuedBy,
            licenseResult.Value.IssuedAt,
            validUntil);
        
        if(licenseUpdateResult.IsFailure)
            return Result.Failure(licenseUpdateResult.Error);
        
        return await _licenseRepository.Update(licenseUpdateResult.Value);
    }
    
    public async Task<Result<EmployeeLicense>> GetLicense(Guid id)
    {
        return await _licenseRepository.GetById(id);
    }

    public async Task<Result<List<EmployeeLicense>>> GetEmployeeLicenses(Guid employeeId)
    {
        return await _licenseRepository.GetByEmployeeId(employeeId);
    }
    
    public async Task<Result<List<EmployeeLicense>>> GetEmployeeLicensesValidOnDate(Guid employeeId, DateOnly asOfDate)
    {
        return await _licenseRepository.GetValidLicenses(employeeId, asOfDate);
    }
    
    public async Task<Result<List<EmployeeLicense>>> Filter(
        Guid? employeeId, 
        string? licenseNumber,
        string? issuedBy,
        DateOnly? issuedFrom,
        DateOnly? issuedTo,
        DateOnly? validUntilFrom,
        DateOnly? validUntilTo)
    {
        var employeeLicenseFilter = EmployeeLicenseFilter.Create(
            employeeId,
            licenseNumber,
            issuedBy,
            issuedFrom,
            issuedTo,
            validUntilFrom,
            validUntilTo);
        
        if(employeeLicenseFilter.IsFailure)
            return Result.Failure<List<EmployeeLicense>>(employeeLicenseFilter.Error);
        
        return await _licenseRepository.Filter(employeeLicenseFilter.Value);
    }
}