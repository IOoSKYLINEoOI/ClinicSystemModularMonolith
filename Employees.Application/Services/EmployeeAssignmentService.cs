using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Interfaces.Services;
using Employees.Core.Models;

namespace Employees.Application.Services;

public class EmployeeAssignmentService : IEmployeeAssignmentService
{
    private readonly IEmployeeAssignmentRepository _employeeAssignmentRepository;

    public EmployeeAssignmentService(IEmployeeAssignmentRepository employeeAssignmentRepository)
    {
        _employeeAssignmentRepository = employeeAssignmentRepository;
    }

    public async Task<Result> AddAssignment(
        Guid employeeId,
        int positionId,
        Guid departmentId,
        DateOnly assignedAt,
        DateOnly? removedAt)
        
    {
        var assignmentResult = EmployeeAssignment.Create(
            id: Guid.NewGuid(),
            employeeId,
            positionId,
            departmentId,
            assignedAt,
            removedAt);
        
        if(assignmentResult.IsFailure)
            return Result.Failure(assignmentResult.Error);
        
        await _employeeAssignmentRepository.Add(assignmentResult.Value);
        
        return Result.Success();
    }

    public async Task<Result> UpdateAssignment(
        Guid id,
        Guid employeeId,
        int positionId,
        Guid departmentId,
        DateOnly assignedAt,
        DateOnly? removedAt)
    {
        var assignmentResult = await _employeeAssignmentRepository.GetById(id);
        if(assignmentResult.IsFailure)
            return Result.Failure(assignmentResult.Error);
        
        var assignmentUpdateResult = EmployeeAssignment.Create(
            assignmentResult.Value.Id,
            employeeId,
            positionId,
            departmentId,
            assignedAt,
            removedAt);
        if(assignmentUpdateResult.IsFailure)
            return Result.Failure(assignmentUpdateResult.Error);
        
        return await _employeeAssignmentRepository.Update(assignmentUpdateResult.Value);
    }

    public async Task<Result<EmployeeAssignment>> GetAssignment(Guid employeeId)
    {
        return await _employeeAssignmentRepository.GetById(employeeId);
    }

    public async Task<Result<List<EmployeeAssignment>>> GetEmployeeAssignments(Guid employeeId)
    {
        return await _employeeAssignmentRepository.GetByEmployeeId(employeeId);
    }

    public async Task<Result<List<EmployeeAssignment>>> GetEmployeeByDepartmentAssignments(Guid departmentId,
        DateOnly onDate)
    {
        return await _employeeAssignmentRepository.GetActiveByDepartment(departmentId, onDate);
    }

    public async Task<Result<List<EmployeeAssignment>>> GetAllEmployeeAssignment()
    {
        return await _employeeAssignmentRepository.GetAll();
    }
}