using CSharpFunctionalExtensions;
using Employees.Core.Models;
using Employees.Core.Models.Filter;

namespace Employees.Core.Interfaces.Repositories;

public interface IEmployeeCertificateRepository
{
    Task Add(EmployeeCertificate certificate);
    Task<Result> Update(EmployeeCertificate certificate);
    Task<Result<EmployeeCertificate>> GetById(Guid id);
    Task<Result<List<EmployeeCertificate>>> GetByEmployeeId(Guid employeeId);
    Task<Result<List<EmployeeCertificate>>> GetValidCertificates(Guid employeeId, DateOnly asOfDate);
    Task<Result<List<EmployeeCertificate>>> Filter(EmployeeCertificateFilter filter);
}