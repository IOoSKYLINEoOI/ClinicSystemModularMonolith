namespace Employees.Core.Models.Filter;

public class EmployeeCertificateFilter
{
    public Guid? EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? IssuedBy { get; set; }
    public DateOnly? IssuedFrom { get; set; }
    public DateOnly? IssuedTo { get; set; }
    public DateOnly? ValidUntilFrom { get; set; }
    public DateOnly? ValidUntilTo { get; set; }
}