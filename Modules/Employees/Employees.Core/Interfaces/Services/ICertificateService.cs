using CSharpFunctionalExtensions;
using Employees.Core.Models;

namespace Employees.Core.Interfaces.Services;

public interface ICertificateService
{
    Task<Result> AddCertificate(
        Guid employeeId,
        string name,
        string issuedBy,
        DateOnly issuedAt,
        DateOnly? validUntil);

    Task<Result> UpdateCertificate(
        Guid id,
        string name,
        string issuedBy,
        DateOnly? validUntil);

    Task<Result<EmployeeCertificate>> GetCertificate(Guid id);
    Task<Result<List<EmployeeCertificate>>> GetEmployeeCertificates(Guid employeeId);
    Task<Result<List<EmployeeCertificate>>> GetEmployeeCertificatesValidOnDate(Guid employeeId, DateOnly asOfDate);

    Task<Result<List<EmployeeCertificate>>> Filter(
        Guid? employeeId, 
        string? name,
        string? issuedBy,
        DateOnly? issuedFrom,
        DateOnly? issuedTo,
        DateOnly? validUntilFrom,
        DateOnly? validUntilTo);
}