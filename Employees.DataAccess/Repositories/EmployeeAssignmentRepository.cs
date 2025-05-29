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

    public async Task Add(EmployeeAssignment assignment)
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
    }

    public async Task Update(EmployeeAssignment assignment)
    {
        var assignmentEntity = await _context.EmployeeAssignments
            .FirstOrDefaultAsync(x => x.Id == assignment.Id)
            ?? throw new InvalidOperationException("Assignment not found.");

        assignmentEntity.EmployeeId = assignment.EmployeeId;
        assignmentEntity.PositionId = assignment.PositionId;
        assignmentEntity.DepartmentId = assignment.DepartmentId;
        assignmentEntity.AssignedAt = assignment.AssignedAt;
        assignmentEntity.RemovedAt = assignment.RemovedAt;

        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeAssignment> GetById(Guid id)
    {
        var assignmentEntity = await _context.EmployeeAssignments
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new InvalidOperationException("Assignment not found.");

        var assignment = EmployeeAssignment.Create
        (
            assignmentEntity.Id,
            assignmentEntity.EmployeeId,
            assignmentEntity.PositionId,
            assignmentEntity.DepartmentId,
            assignmentEntity.AssignedAt,
            assignmentEntity.RemovedAt
        );
        
        if(!assignment.IsSuccess)
            throw new InvalidOperationException(assignment.Error);
        
        return assignment.Value;
    }

    public async Task<List<EmployeeAssignment>> GetByEmployeeId(Guid employeeId)
    {
        var assignmentsEntityes = await _context.EmployeeAssignments
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.Position)
            .ToListAsync();

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
            
            if(assignment.IsSuccess)
                assignments.Add(assignment.Value);
        }
        
        return assignments;
    }

    public async Task<List<EmployeeAssignment>> GetActiveByDepartment(Guid departmentId, DateOnly onDate)
    {
        var assignmentsEntityes = await _context.EmployeeAssignments
            .AsNoTracking()
            .Where(x => x.DepartmentId == departmentId &&
                        x.AssignedAt <= onDate &&
                        (x.RemovedAt == null || x.RemovedAt > onDate))
            .Include(x => x.Employee)
            .Include(x => x.Position)
            .ToListAsync();
        
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
            
            if(assignment.IsSuccess)
                assignments.Add(assignment.Value);
        }
        
        return assignments;
    }

    public async Task<List<EmployeeAssignment>> GetAll()
    {
        var assignmentsEntityes = await _context.EmployeeAssignments
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Position)
            .ToListAsync();
        
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
            
            if(assignment.IsSuccess)
                assignments.Add(assignment.Value);
        }
        
        return assignments;
    }
}