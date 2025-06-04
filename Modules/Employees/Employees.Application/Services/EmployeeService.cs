using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Interfaces.Services;
using Employees.Core.Models;

namespace Employees.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Result> AddEmployee(
        Guid userId,
        DateOnly hireDate,
        DateOnly? fireDate)
    {
        var employeeResult = Employee.Create(
            id: Guid.NewGuid(),
            userId,
            hireDate,
            fireDate,
            isActive: true);
        
        if(employeeResult.IsFailure)
            return Result.Failure(employeeResult.Error);
        
        await _employeeRepository.Add(employeeResult.Value);
        return Result.Success();
    }

    public async Task<Result> UpdateEmployee(
        Guid id,
        Guid userId,
        DateOnly hireDate, 
        DateOnly fireDate)
    {
        var employeeResult = await _employeeRepository.GetById(id);
        if(employeeResult.IsFailure)
            return Result.Failure(employeeResult.Error);
        
        var employeeUpdateResult = Employee.Create(
            employeeResult.Value.Id,
            userId,
            hireDate,
            fireDate,
            employeeResult.Value.IsActive);
        if(employeeUpdateResult.IsFailure)
            return Result.Failure(employeeUpdateResult.Error);
        
        return await _employeeRepository.Update(employeeUpdateResult.Value);
    }

    public async Task<Result<Employee>> GetEmployee(Guid id)
    {
        return await _employeeRepository.GetById(id);
    }

    public async Task<Result> FireEmployee(
        Guid id,
        DateOnly hireDate)
    {
        return await _employeeRepository.FireEmployee(id, hireDate);
    }

    public async Task<Result<List<Employee>>> AllEmployees()
    {
        return await _employeeRepository.GetAll();
    }

    public async Task<Result<bool>> GetIsActive(Guid id)
    {
        return await _employeeRepository.GetIsActive(id);
    }

    public async Task<Result<Employee>> GetEmployeesByUserId(Guid userId)
    {
        return await _employeeRepository.GetByUserId(userId);
    }
    
    public async Task<Result<List<Employee>>> GetEmployeeRecentHired(int days = 30)
    {
        return await _employeeRepository.GetRecentlyHired(days);
    }
}