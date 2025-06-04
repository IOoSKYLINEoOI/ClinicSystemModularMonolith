using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Models;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _context;

    public EmployeeRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Add(Employee employee)
    {
        var employeeEntity = new EmployeeEntity()
        {
            Id = employee.Id,
            UserId = employee.UserId,
            HireDate = employee.HireDate,
            FireDate = employee.FireDate,
            IsActive = employee.IsActive,
        };
        
        await _context.Employees.AddAsync(employeeEntity);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> Update(Employee employeeNew)
    {
        var employeeEntity = await _context.Employees
            .FirstOrDefaultAsync(u => u.Id == employeeNew.Id && u.IsActive);
        
        if(employeeEntity is null)
            return Result.Failure("Employee not found or deleted");
        
        employeeEntity.UserId = employeeNew.UserId;
        employeeEntity.HireDate = employeeNew.HireDate;
        employeeEntity.FireDate = employeeNew.FireDate;
        
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<Employee>> GetById(Guid id)
    {
        var employeeEntity = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        
        if(employeeEntity is null)
            return Result.Failure<Employee>("Employee not found or deleted");
                             

        var employeeResult = Employee.Create(
            employeeEntity.Id,
            employeeEntity.UserId,
            employeeEntity.HireDate,
            employeeEntity.FireDate,
            employeeEntity.IsActive);
        
        if (!employeeResult.IsSuccess)
            return Result.Failure<Employee>(employeeResult.Error);
        
        return employeeResult;
    }

    public async Task<Result> FireEmployee(Guid employeeId, DateOnly fireDate)
    {
        var employeeEntity = await _context.Employees
                                 .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsActive);
        if (employeeEntity is null)
            return Result.Failure<Employee>("Employee not found or deleted");

        employeeEntity.FireDate = fireDate;
        employeeEntity.IsActive = false;

        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<List<Employee>>> GetAll()
    {
        var entities = await _context.Employees
            .AsNoTracking()
            .Where(u => u.IsActive)
            .ToListAsync();
        
        if(entities.Count == 0)
            return Result.Failure<List<Employee>>("Employees not found or deleted");
        
        var result = new List<Employee>();

        foreach (var entity in entities)
        {
            var employee = Employee.Create(
                entity.Id, 
                entity.UserId, 
                entity.HireDate, 
                entity.FireDate, 
                entity.IsActive);
            
            if (!employee.IsSuccess)
                return Result.Failure<List<Employee>>(employee.Error);
            
            result.Add(employee.Value);
        }
        
        return result;
    }

    public async Task<Result<bool>> GetIsActive(Guid id)
    {
        var exists = await _context.Employees.AnyAsync(e => e.Id == id && e.IsActive);
        return Result.Success(exists);
    }

    public async Task<Result<Employee>> GetByUserId(Guid userId)
    {
        var employeeEntity = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId);
        
        if (employeeEntity is null)
            return Result.Failure<Employee>("Employee not found or deleted");
        
        var employee = Employee.Create(
            employeeEntity.Id, 
            employeeEntity.UserId, 
            employeeEntity.HireDate,
            employeeEntity.FireDate,
            employeeEntity.IsActive);
        
        if(!employee.IsSuccess)
            return Result.Failure<Employee>(employee.Error);
        
        return employee;
    }
    
    public async Task<Result<List<Employee>>> GetRecentlyHired(int days)
    {
        var since = DateOnly.FromDateTime(DateTime.Now.AddDays(-days));

        var entities = await _context.Employees
            .AsNoTracking()
            .Where(e => e.HireDate >= since && e.IsActive)
            .ToListAsync();
        
        if(entities.Count == 0)
            return Result.Failure<List<Employee>>("Employees not found or deleted");

        var result = new List<Employee>();

        foreach (var entity in entities)
        {
            var employee = Employee.Create(
                entity.Id, 
                entity.UserId, 
                entity.HireDate, 
                entity.FireDate, 
                entity.IsActive);
            
            if (!employee.IsSuccess)
                return Result.Failure<List<Employee>>(employee.Error);
            
            result.Add(employee.Value);
        }
        
        return result;
    }
}