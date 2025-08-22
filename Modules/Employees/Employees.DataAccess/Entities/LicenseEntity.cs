namespace Employees.DataAccess.Entities;

public class LicenseEntity
{
    public Guid Id { get; set; }
    
    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    
    public string Number { get; set; } = null!;
    public string IssuedBy { get; set; } = null!;
    
    public DateOnly IssuedAt { get; set; }
    public DateOnly? ValidUntil { get; set; }
}