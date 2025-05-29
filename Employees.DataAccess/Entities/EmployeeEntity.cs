namespace Employees.DataAccess.Entities;

public class EmployeeEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public DateOnly HireDate { get; set; }
    public DateOnly? FireDate { get; set; }
    
    public bool IsActive { get; set; }
    
    public ICollection<EmployeeAssignmentEntity> EmployeeAssignments { get; set; } = new List<EmployeeAssignmentEntity>();
    public ICollection<EmployeeLicenseEntity> EmployeeLicenses { get; set; } = new List<EmployeeLicenseEntity>();
    public ICollection<EmployeeCertificateEntity> EmployeeCertificates { get; set; } = new List<EmployeeCertificateEntity>();
}