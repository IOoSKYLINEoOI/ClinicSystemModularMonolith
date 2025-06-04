namespace Employees.DataAccess.Entities;

public class EmployeeCertificateEntity
{
    public Guid Id { get; set; }
    
    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public string IssuedBy { get; set; } = null!;
    
    public DateOnly IssuedAt { get; set; }
    public DateOnly? ValidUntil { get; set; }
}