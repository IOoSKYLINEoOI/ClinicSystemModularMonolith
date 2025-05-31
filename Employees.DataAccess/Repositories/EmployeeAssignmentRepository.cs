using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Models;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess.Repositories;

public class EmployeeAssignmentRepository : IEmployeeAssignmentRepository
{
    private readonly EmployeeDbContext _context;

    public EmployeeAssignmentRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Add(EmployeeAssignment assignment)
    {
        var assignmentEntity = new EmployeeAssignmentEntity()
        {
            Id = assignment.Id,
            EmployeeId = assignment.EmployeeId,
            PositionId = assignment.PositionId,
            DepartmentId = assignment.DepartmentId,
            AssignedAt = assignment.AssignedAt,
            RemovedAt = assignment.RemovedAt
        };
        
        await _context.EmployeeAssignments.AddAsync(assignmentEntity);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> Update(EmployeeAssignment assignment)
    {
        var assignmentEntity = await _context.EmployeeAssignments
            .FirstOrDefaultAsync(x => x.Id == assignment.Id);
        
        if (assignmentEntity == null)
            return Result.Failure("Employee assignment not found");

        assignmentEntity.EmployeeId = assignment.EmployeeId;
        assignmentEntity.PositionId = assignment.PositionId;
        assignmentEntity.DepartmentId = assignment.DepartmentId;
        assignmentEntity.AssignedAt = assignment.AssignedAt;
        assignmentEntity.RemovedAt = assignment.RemovedAt;

        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<EmployeeAssignment>> GetById(Guid id)
    {
        var assignmentEntity = await _context.EmployeeAssignments
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (assignmentEntity == null)
            return Result.Failure<EmployeeAssignment>("Employee assignment not found");
        
        var assignmentResult = EmployeeAssignment.Create
        (
            assignmentEntity.Id,
            assignmentEntity.EmployeeId,
            assignmentEntity.PositionId,
            assignmentEntity.DepartmentId,
            assignmentEntity.AssignedAt,
            assignmentEntity.RemovedAt
        );
        
        if(!assignmentResult.IsSuccess)
            return Result.Failure<EmployeeAssignment>(assignmentResult.Error);
        
        return assignmentResult;
    }

    public async Task<Result<List<EmployeeAssignment>>> GetByEmployeeId(Guid employeeId)
    {
        var assignmentsEntityes = await _context.EmployeeAssignments
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.Position)
            .ToListAsync();
        
        if(assignmentsEntityes.Count == 0)
            return Result.Failure<List<EmployeeAssignment>>("Employee assignment not found");

        var assignments = new List<EmployeeAssignment>();

        foreach (var assignmentEntity in assignmentsEntityes)
        {
            var assignment = EmployeeAssignment.Create(
                assignmentEntity.Id,
                assignmentEntity.EmployeeId,
                assignmentEntity.PositionId,
                assignmentEntity.DepartmentId,
                assignmentEntity.AssignedAt,
                assignmentEntity.RemovedAt);
            
            if(!assignment.IsSuccess)
                return Result.Failure<List<EmployeeAssignment>>("Employee assignment not found");
            
            assignments.Add(assignment.Value);
        }
        
        return assignments;
    }

    public async Task<Result<List<EmployeeAssignment>>> GetActiveByDepartment(Guid departmentId, DateOnly onDate)
    {
        var assignmentsEntityes = await _context.EmployeeAssignments
            .AsNoTracking()
            .Where(x => x.DepartmentId == departmentId &&
                        x.AssignedAt <= onDate &&
                        (x.RemovedAt == null || x.RemovedAt > onDate))
            .Include(x => x.Employee)
            .Include(x => x.Position)
            .ToListAsync();
        
        if(assignmentsEntityes.Count == 0)
            return Result.Failure<List<EmployeeAssignment>>("Employee assignment not found");
        
        var assignments = new List<EmployeeAssignment>();

        foreach (var assignmentEntity in assignmentsEntityes)
        {
            var assignment = EmployeeAssignment.Create(
                assignmentEntity.Id,
                assignmentEntity.EmployeeId,
                assignmentEntity.PositionId,
                assignmentEntity.DepartmentId,
                assignmentEntity.AssignedAt,
                assignmentEntity.RemovedAt);
            
            if(!assignment.IsSuccess)
                return Result.Failure<List<EmployeeAssignment>>("Employee assignment not found");
            
            assignments.Add(assignment.Value);
        }
        
        return assignments;
    }

    public async Task<Result<List<EmployeeAssignment>>> GetAll()
    {
        var assignmentsEntityes = await _context.EmployeeAssignments
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Position)
            .ToListAsync();
        
        if(assignmentsEntityes.Count == 0)
            return Result.Failure<List<EmployeeAssignment>>("Employee assignment not found");
        
        var assignments = new List<EmployeeAssignment>();

        foreach (var assignmentEntity in assignmentsEntityes)
        {
            var assignment = EmployeeAssignment.Create(
                assignmentEntity.Id,
                assignmentEntity.EmployeeId,
                assignmentEntity.PositionId,
                assignmentEntity.DepartmentId,
                assignmentEntity.AssignedAt,
                assignmentEntity.RemovedAt);
            
            if(!assignment.IsSuccess)
                return Result.Failure<List<EmployeeAssignment>>("Employee assignment not found");
            
            assignments.Add(assignment.Value);
        }
        
        return assignments;
    }
}