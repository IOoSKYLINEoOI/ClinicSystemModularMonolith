using CSharpFunctionalExtensions;

namespace Employees.Core.Models;

public class EmployeeAssignment
{
    private static readonly DateOnly MaxDeltaDate = DateOnly.FromDateTime(DateTime.Now.AddDays(90));
    private static readonly DateOnly MinDeltaDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-90));
    
    private EmployeeAssignment(
        Guid id, 
        Guid employeeId, 
        int positionId, 
        Guid departmentId, 
        DateOnly assignedAt, 
        DateOnly? removedAt)
    {
        Id = id;
        EmployeeId = employeeId;
        PositionId = positionId;
        DepartmentId = departmentId;
        AssignedAt = assignedAt;
        RemovedAt = removedAt;
    }
    
    public Guid Id { get; }
    public Guid EmployeeId { get; }
    public int PositionId { get; }
    public Guid DepartmentId { get; }
    public DateOnly AssignedAt { get; }
    public DateOnly? RemovedAt { get; }

    public static Result<EmployeeAssignment> Create(
        Guid id,
        Guid employeeId,
        int positionId,
        Guid departmentId,
        DateOnly assignedAt,
        DateOnly? removedAt)
    {
        if (employeeId == Guid.Empty || departmentId == Guid.Empty)
        {
            return Result.Failure<EmployeeAssignment>(
                $"Invalid references: all foreign keys must be non-empty GUIDs.");
        }
        
        if (removedAt.HasValue && removedAt.Value < assignedAt)
        {
            return Result.Failure<EmployeeAssignment>(
                $"'RemovedAt' cannot be earlier than 'AssignedAt'.");
        }
        
        if (assignedAt.CompareTo(MaxDeltaDate) > 0 || assignedAt.CompareTo(MinDeltaDate) < 0)
        {
            return Result.Failure<EmployeeAssignment>(
                $"'{nameof(assignedAt)}' is out of range. Must be between {MinDeltaDate.ToString("yyyy-MM-dd")} and {MaxDeltaDate.ToString("yyyy-MM-dd")}.");
        }
        
        if (removedAt.HasValue)
        {
            if (removedAt.Value.CompareTo(MaxDeltaDate) > 0 || removedAt.Value.CompareTo(MinDeltaDate) < 0)
            {
                return Result.Failure<EmployeeAssignment>(
                    $"'{nameof(removedAt)}' is out of range. Must be between {MinDeltaDate:yyyy-MM-dd} and {MaxDeltaDate:yyyy-MM-dd}.");
            }
        }
        
        var employeeAssigment = new EmployeeAssignment(
            id, 
            employeeId, 
            positionId, 
            departmentId, 
            assignedAt, 
            removedAt);
        
        return Result.Success(employeeAssigment);
    }
}