using Employees.Core.Models;

namespace Employees.Core.Interfaces.Repositories;

public interface IEmployeeAssignmentRepository
{
    Task Add(EmployeeAssignment assignment);
    Task Update(EmployeeAssignment assignment);
    Task<EmployeeAssignment> GetById(Guid id);
    Task<List<EmployeeAssignment>> GetByEmployeeId(Guid employeeId);
    Task<List<EmployeeAssignment>> GetActiveByDepartment(Guid departmentId, DateOnly onDate);
    Task<List<EmployeeAssignment>> GetAll();
}