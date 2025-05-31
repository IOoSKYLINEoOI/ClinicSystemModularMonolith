using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Interfaces.Services;
using Employees.Core.Models;
using Employees.Core.Models.Filter;

namespace Employees.Application.Services;

public class EmployeeCertificateService : IEmployeeCertificateService
{
    private readonly IEmployeeCertificateRepository _employeeCertificateRepository;

    public EmployeeCertificateService(
        IEmployeeCertificateRepository employeeCertificateRepository)
    {
        _employeeCertificateRepository = employeeCertificateRepository;
    }

    public async Task<Result> AddCertificate(
        Guid employeeId,
        string name,
        string issuedBy,
        DateOnly issuedAt,
        DateOnly? validUntil)
    {
        var certificateResult = EmployeeCertificate.Create(
            id: Guid.NewGuid(),
            employeeId,
            name,
            issuedBy,
            issuedAt,
            validUntil);
        
        if(certificateResult.IsFailure)
            return Result.Failure(certificateResult.Error);
        
        await _employeeCertificateRepository.Add(certificateResult.Value);
        return Result.Success();
    }

    public async Task<Result> UpdateCertificate(
        Guid id,
        string name,
        string issuedBy,
        DateOnly? validUntil)
    {
        var certificateResult = await _employeeCertificateRepository.GetById(id);
        if (certificateResult.IsFailure)
            return Result.Failure(certificateResult.Error);

        var certificateUpdateResult = EmployeeCertificate.Create(
            id,
            certificateResult.Value.EmployeeId,
            name,
            issuedBy,
            certificateResult.Value.IssuedAt,
            validUntil);

        if (certificateUpdateResult.IsFailure)
            return Result.Failure(certificateUpdateResult.Error);

        return await _employeeCertificateRepository.Update(certificateUpdateResult.Value);
    }

    public async Task<Result<EmployeeCertificate>> GetCertificate(Guid id)
    {
        return await _employeeCertificateRepository.GetById(id);
    }

    public async Task<Result<List<EmployeeCertificate>>> GetEmployeeCertificates(Guid employeeId)
    {
        return await _employeeCertificateRepository.GetByEmployeeId(employeeId);
    }
    
    public async Task<Result<List<EmployeeCertificate>>> GetEmployeeCertificatesValidOnDate(Guid employeeId, DateOnly asOfDate)
    {
        return await _employeeCertificateRepository.GetValidCertificates(employeeId, asOfDate);
    }
    
    public async Task<Result<List<EmployeeCertificate>>> Filter(
        Guid? employeeId, 
        string? name,
        string? issuedBy,
        DateOnly? issuedFrom,
        DateOnly? issuedTo,
        DateOnly? validUntilFrom,
        DateOnly? validUntilTo)
    {
        var employeeCertificateFilter = EmployeeCertificateFilter.Create(
            employeeId,
            name,
            issuedBy,
            issuedFrom,
            issuedTo,
            validUntilFrom,
            validUntilTo);
        
        if(employeeCertificateFilter.IsFailure)
            return Result.Failure<List<EmployeeCertificate>>(employeeCertificateFilter.Error);
        
        return await _employeeCertificateRepository.Filter(employeeCertificateFilter.Value);
    }
}