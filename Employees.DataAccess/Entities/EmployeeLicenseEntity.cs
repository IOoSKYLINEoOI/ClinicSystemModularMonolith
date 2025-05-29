namespace Employees.DataAccess.Entities;

public class EmployeeLicenseEntity
{
    public Guid Id { get; set; }
    
    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    
    public string LicenseNumber { get; set; } = null!;
    public string IssuedBy { get; set; } = null!;
    
    public DateOnly IssuedAt { get; set; }
    public DateOnly? ValidUntil { get; set; }
}