using Employees.Core.Models;
using Employees.Core.Models.Filter;

namespace Employees.Core.Interfaces.Repositories;

public interface IEmployeeLicenseRepository
{
    Task Add(EmployeeLicense license);
    Task Update(EmployeeLicense license);
    Task<EmployeeLicense> GetById(Guid id);
    Task<List<EmployeeLicense>> GetByEmployeeId(Guid employeeId);
    Task<List<EmployeeLicense>> GetValidLicenses(Guid employeeId, DateOnly asOfDate);
    Task<List<EmployeeLicense>> Filter(EmployeeLicenseFilter filter);
}