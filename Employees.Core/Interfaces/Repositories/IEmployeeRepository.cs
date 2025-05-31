using CSharpFunctionalExtensions;
using Employees.Core.Models;

namespace Employees.Core.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<Result> Add(Employee employee);
    Task<Result> Update(Employee employeeNew);
    Task<Result<Employee>> GetById(Guid id);
    Task<Result> FireEmployee(Guid employeeId, DateOnly fireDate);
    Task<Result<List<Employee>>> GetAll();
    Task<Result<bool>> GetIsActive(Guid id);
    Task<Result<Employee>> GetByUserId(Guid userId);
    Task<Result<List<Employee>>> GetRecentlyHired(int days);
}