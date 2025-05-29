using Employees.Core.Models;

namespace Employees.Core.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task Add(Employee employee);
    Task Update(Employee employeeNew);
    Task<Employee> GetById(Guid id);
    Task FireEmployee(Guid employeeId, DateTime fireDate);
    Task<List<Employee>> GetAll();
    Task<bool> GetIsActive(Guid id);
    Task<Employee> GetByUserId(Guid userId);
    Task<List<Employee>> GetRecentlyHired(int days = 30);
}