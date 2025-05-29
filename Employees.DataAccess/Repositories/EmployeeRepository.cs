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

    public async Task Add(Employee employee)
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
    }

    public async Task Update(Employee employeeNew)
    {
        var employee = await _context.Employees
                           .FirstOrDefaultAsync(u => u.Id == employeeNew.Id && u.IsActive)
                       ?? throw new InvalidOperationException("User not found or deleted.");
           
        
        employee.UserId = employeeNew.UserId;
        employee.HireDate = employeeNew.HireDate;
        employee.FireDate = employeeNew.FireDate;
        employee.IsActive = employeeNew.IsActive;
        
        await _context.SaveChangesAsync();
    }

    public async Task<Employee> GetById(Guid id)
    {
        var employeeEntity = await _context.Employees
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(u => u.Id == id && u.IsActive)
                             ?? throw new InvalidOperationException("Employee not found or isn't active.");

        var employee = Employee.Create(
            employeeEntity.Id,
            employeeEntity.UserId,
            employeeEntity.HireDate,
            employeeEntity.FireDate,
            employeeEntity.IsActive);
        
        if (!employee.IsSuccess)
            throw new InvalidOperationException(employee.Error);
        
        return employee.Value;
    }

    public async Task FireEmployee(Guid employeeId, DateTime fireDate)
    {
        var employeeEntity = await _context.Employees
                                 .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsActive)
                             ?? throw new InvalidOperationException("Employee not found or isn't active.");

        var updated = Employee.Create(
            employeeEntity.Id,
            employeeEntity.UserId,
            employeeEntity.HireDate,
            DateOnly.FromDateTime(fireDate),
            false
        );

        if (!updated.IsSuccess)
            throw new InvalidOperationException(updated.Error);

        employeeEntity.FireDate = DateOnly.FromDateTime(fireDate);
        employeeEntity.IsActive = false;

        await _context.SaveChangesAsync();
    }

    public async Task<List<Employee>> GetAll()
    {
        var entities = await _context.Employees
            .AsNoTracking()
            .Where(u => u.IsActive)
            .ToListAsync();
        
        var result = new List<Employee>();

        foreach (var entity in entities)
        {
            var employee = Employee.Create(
                entity.Id, 
                entity.UserId, 
                entity.HireDate, 
                entity.FireDate, 
                entity.IsActive);
            
            if (employee.IsSuccess)
                result.Add(employee.Value);
        }
        
        return result;
    }

    public async Task<bool> GetIsActive(Guid id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }

    public async Task<Employee> GetByUserId(Guid userId)
    {
        var employeeEntity = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId)
                             ?? throw new InvalidOperationException("Employee not found or isn't active.");
        
        var employee = Employee.Create(
            employeeEntity.Id, 
            employeeEntity.UserId, 
            employeeEntity.HireDate,
            employeeEntity.FireDate,
            employeeEntity.IsActive);
        
        if(!employee.IsSuccess)
            throw new InvalidOperationException(employee.Error);
        
        return employee.Value;
    }
    
    public async Task<List<Employee>> GetRecentlyHired(int days = 30)
    {
        var since = DateOnly.FromDateTime(DateTime.Now.AddDays(-days));

        var entities = await _context.Employees
            .AsNoTracking()
            .Where(e => e.HireDate >= since && e.IsActive)
            .ToListAsync();

        return entities
            .Select(e => Employee.Create(e.Id, e.UserId, e.HireDate, e.FireDate, e.IsActive))
            .Where(r => r.IsSuccess)
            .Select(r => r.Value)
            .ToList();
    }
}