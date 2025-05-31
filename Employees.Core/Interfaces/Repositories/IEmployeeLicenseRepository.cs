using CSharpFunctionalExtensions;
using Employees.Core.Models;
using Employees.Core.Models.Filter;

namespace Employees.Core.Interfaces.Repositories;

public interface IEmployeeLicenseRepository
{
    Task Add(EmployeeLicense license);
    Task<Result> Update(EmployeeLicense license);
    Task<Result<EmployeeLicense>> GetById(Guid id);
    Task<Result<List<EmployeeLicense>>> GetByEmployeeId(Guid employeeId);
    Task<Result<List<EmployeeLicense>>> GetValidLicenses(Guid employeeId, DateOnly asOfDate);
    Task<Result<List<EmployeeLicense>>> Filter(EmployeeLicenseFilter filter);
}