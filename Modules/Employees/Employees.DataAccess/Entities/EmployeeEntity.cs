namespace Employees.DataAccess.Entities;

public class EmployeeEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public DateOnly HireDate { get; set; }
    public DateOnly? FireDate { get; set; }
    
    public bool IsActive { get; set; }
    
    public ICollection<AssignmentEntity> EmployeeAssignments { get; set; } = new List<AssignmentEntity>();
    public ICollection<LicenseEntity> EmployeeLicenses { get; set; } = new List<LicenseEntity>();
    public ICollection<CertificateEntity> EmployeeCertificates { get; set; } = new List<CertificateEntity>();
}