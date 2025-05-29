namespace Employees.DataAccess.Entities;

public class PositionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<EmployeeAssignmentEntity> Assignments { get; set; } = new List<EmployeeAssignmentEntity>();
}