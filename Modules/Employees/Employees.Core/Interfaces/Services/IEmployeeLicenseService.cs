using CSharpFunctionalExtensions;
using Employees.Core.Models;

namespace Employees.Core.Interfaces.Services;

public interface IEmployeeLicenseService
{
    Task<Result> AddLicense(
        Guid employeeId,
        string licenseNumber,
        string issuedBy,
        DateOnly issuedAt,
        DateOnly? validUntil);

    Task<Result> UpdateLicense(
        Guid id,
        string licenseNumber,
        string issuedBy,
        DateOnly? validUntil);

    Task<Result<EmployeeLicense>> GetLicense(Guid id);
    Task<Result<List<EmployeeLicense>>> GetEmployeeLicenses(Guid employeeId);
    Task<Result<List<EmployeeLicense>>> GetEmployeeLicensesValidOnDate(Guid employeeId, DateOnly asOfDate);

    Task<Result<List<EmployeeLicense>>> Filter(
        Guid? employeeId, 
        string? licenseNumber,
        string? issuedBy,
        DateOnly? issuedFrom,
        DateOnly? issuedTo,
        DateOnly? validUntilFrom,
        DateOnly? validUntilTo);
}