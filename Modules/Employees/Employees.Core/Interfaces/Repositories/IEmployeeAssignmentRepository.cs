using CSharpFunctionalExtensions;
using Employees.Core.Models;

namespace Employees.Core.Interfaces.Repositories;

public interface IEmployeeAssignmentRepository
{
    Task<Result> Add(EmployeeAssignment assignment);
    Task<Result> Update(EmployeeAssignment assignment);
    Task<Result<EmployeeAssignment>> GetById(Guid id);
    Task<Result<List<EmployeeAssignment>>> GetByEmployeeId(Guid employeeId);
    Task<Result<List<EmployeeAssignment>>> GetActiveByDepartment(Guid departmentId, DateOnly onDate);
    Task<Result<List<EmployeeAssignment>>> GetAll();
}