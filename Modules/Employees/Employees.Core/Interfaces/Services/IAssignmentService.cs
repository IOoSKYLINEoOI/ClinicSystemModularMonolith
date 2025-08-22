using CSharpFunctionalExtensions;
using Employees.Core.Models;

namespace Employees.Core.Interfaces.Services;

public interface IAssignmentService
{
    Task<Result> AddAssignment(
        Guid employeeId,
        int positionId,
        Guid departmentId,
        DateOnly assignedAt,
        DateOnly? removedAt);

    Task<Result> UpdateAssignment(
        Guid id,
        Guid employeeId,
        int positionId,
        Guid departmentId,
        DateOnly assignedAt,
        DateOnly? removedAt);

    Task<Result<EmployeeAssignment>> GetAssignment(Guid employeeId);
    Task<Result<List<EmployeeAssignment>>> GetEmployeeAssignments(Guid employeeId);

    Task<Result<List<EmployeeAssignment>>> GetEmployeeByDepartmentAssignments(Guid departmentId,
        DateOnly onDate);

    Task<Result<List<EmployeeAssignment>>> GetAllEmployeeAssignment();
}