namespace Employees.DataAccess.Entities;

public class EmployeeAssignmentEntity
{
    public Guid Id { get; set; }
    
    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    public int PositionId { get; set; }
    public PositionEntity Position { get; set; } = null!;
    public Guid DepartmentId { get; set; }
    
    public DateOnly AssignedAt { get; set; }
    public DateOnly? RemovedAt { get; set; }
}   