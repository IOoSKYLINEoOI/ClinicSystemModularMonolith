using CSharpFunctionalExtensions;

namespace Employees.Core.Models;

public class EmployeeCertificate
{
    private const int MaxLength = 255;
    private static readonly DateOnly MaxDeltaDate = DateOnly.FromDateTime(DateTime.Now.AddDays(90));
    private static readonly DateOnly MinDeltaDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-90));
    
    private EmployeeCertificate(
        Guid id, 
        Guid employeeId, 
        string name, 
        string issuedBy, 
        DateOnly issuedAt, 
        DateOnly? validUntil)
    {
        Id = id;
        EmployeeId = employeeId;
        Name = name;
        IssuedBy = issuedBy;
        IssuedAt = issuedAt;
        ValidUntil = validUntil;
    }
    
    public Guid Id { get; }
    public Guid EmployeeId { get; }
    public string Name { get; }
    public string IssuedBy { get; }
    public DateOnly IssuedAt { get; }
    public DateOnly? ValidUntil { get; }

    public static Result<EmployeeCertificate> Create(
        Guid id,
        Guid employeeId,
        string name,
        string issuedBy,
        DateOnly issuedAt,
        DateOnly? validUntil)
    {
        if (employeeId == Guid.Empty )
        {
            return Result.Failure<EmployeeCertificate>(
                $"Invalid references: all foreign keys must be non-empty GUIDs.");
        }
        
        if (string.IsNullOrEmpty(name) || name.Length > MaxLength)
        {
            return Result.Failure<EmployeeCertificate>($"'{nameof(name)}' cannot be null, empty or more than {MaxLength} characters.");
        }
        
        if (string.IsNullOrEmpty(issuedBy) || issuedBy.Length > MaxLength)
        {
            return Result.Failure<EmployeeCertificate>($"'{nameof(issuedBy)}' cannot be null, empty or more than {MaxLength} characters.");
        }
        
        if (validUntil.HasValue && validUntil.Value < issuedAt)
        {
            return Result.Failure<EmployeeCertificate>(
                $"'RemovedAt' cannot be earlier than 'AssignedAt'.");
        }
        
        if (issuedAt.CompareTo(MaxDeltaDate) > 0 || issuedAt.CompareTo(MinDeltaDate) < 0)
        {
            return Result.Failure<EmployeeCertificate>(
                $"'{nameof(issuedAt)}' is out of range. Must be between {MinDeltaDate.ToString("yyyy-MM-dd")} and {MaxDeltaDate.ToString("yyyy-MM-dd")}.");
        }
        
        if (validUntil.HasValue)
        {
            if (validUntil.Value.CompareTo(MaxDeltaDate) > 0 || validUntil.Value.CompareTo(MinDeltaDate) < 0)
            {
                return Result.Failure<EmployeeCertificate>(
                    $"'{nameof(validUntil)}' is out of range. Must be between {MinDeltaDate:yyyy-MM-dd} and {MaxDeltaDate:yyyy-MM-dd}.");
            }
        }
        
        var employeeCertificate = new EmployeeCertificate(
            id, 
            employeeId, 
            name, 
            issuedBy, 
            issuedAt, 
            validUntil);
        
        return Result.Success(employeeCertificate);
    }
}