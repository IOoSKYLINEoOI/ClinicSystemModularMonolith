using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Interfaces.Services;
using Employees.Core.Models;
using Employees.Core.Models.Filter;

namespace Employees.Application.Services;

public class EmployeeLicenseService : IEmployeeLicenseService
{
    private readonly IEmployeeLicenseRepository _employeeLicenseRepository;

    public EmployeeLicenseService(IEmployeeLicenseRepository employeeLicenseRepository)
    {
        _employeeLicenseRepository = employeeLicenseRepository;
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
        
        await _employeeLicenseRepository.Add(licenseResult.Value);
        return Result.Success();
    }

    public async Task<Result> UpdateLicense(
        Guid id,
        string licenseNumber,
        string issuedBy,
        DateOnly? validUntil)
    {
        var licenseResult = await _employeeLicenseRepository.GetById(id);
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
        
        return await _employeeLicenseRepository.Update(licenseUpdateResult.Value);
    }
    
    public async Task<Result<EmployeeLicense>> GetLicense(Guid id)
    {
        return await _employeeLicenseRepository.GetById(id);
    }

    public async Task<Result<List<EmployeeLicense>>> GetEmployeeLicenses(Guid employeeId)
    {
        return await _employeeLicenseRepository.GetByEmployeeId(employeeId);
    }
    
    public async Task<Result<List<EmployeeLicense>>> GetEmployeeLicensesValidOnDate(Guid employeeId, DateOnly asOfDate)
    {
        return await _employeeLicenseRepository.GetValidLicenses(employeeId, asOfDate);
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
        
        return await _employeeLicenseRepository.Filter(employeeLicenseFilter.Value);
    }
}