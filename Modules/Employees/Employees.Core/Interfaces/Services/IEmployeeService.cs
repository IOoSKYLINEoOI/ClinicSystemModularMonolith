using CSharpFunctionalExtensions;
using Employees.Core.Models;

namespace Employees.Core.Interfaces.Services;

public interface IEmployeeService
{
    Task<Result> AddEmployee(
        Guid userId,
        DateOnly hireDate,
        DateOnly? fireDate);

    Task<Result> UpdateEmployee(
        Guid id,
        Guid userId,
        DateOnly hireDate, 
        DateOnly fireDate);

    Task<Result<Employee>> GetEmployee(Guid id);

    Task<Result> FireEmployee(
        Guid id,
        DateOnly hireDate);

    Task<Result<List<Employee>>> AllEmployees();
    Task<Result<bool>> GetIsActive(Guid id);
    Task<Result<Employee>> GetEmployeesByUserId(Guid userId);
    Task<Result<List<Employee>>> GetEmployeeRecentHired(int days = 30);
}