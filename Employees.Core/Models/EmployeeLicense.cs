using CSharpFunctionalExtensions;

namespace Employees.Core.Models;

public class EmployeeLicense
{
    private const int MaxLength = 255;
    private static readonly DateOnly MaxDeltaDate = DateOnly.FromDateTime(DateTime.Now.AddDays(90));
    private static readonly DateOnly MinDeltaDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-90));
    
    private EmployeeLicense(
        Guid id, 
        Guid employeeId, 
        string licenseNumber, 
        string issuedBy, 
        DateOnly issuedAt, 
        DateOnly? validUntil)
    {
        Id = id;
        EmployeeId = employeeId;
        LicenseNumber = licenseNumber;
        IssuedBy = issuedBy;
        IssuedAt = issuedAt;
        ValidUntil = validUntil;
    }
    
    public Guid Id { get; }
    public Guid EmployeeId { get; }
    public string LicenseNumber { get; }
    public string IssuedBy { get; }
    public DateOnly IssuedAt { get; }
    public DateOnly? ValidUntil { get; }

    public static Result<EmployeeLicense> Create(
        Guid id,
        Guid employeeId,
        string licenseNumber,
        string issuedBy,
        DateOnly issuedAt,
        DateOnly? validUntil)
    {
        if (employeeId == Guid.Empty )
        {
            return Result.Failure<EmployeeLicense>(
                $"Invalid references: all foreign keys must be non-empty GUIDs.");
        }
        
        if (string.IsNullOrEmpty(licenseNumber) || licenseNumber.Length > MaxLength)
        {
            return Result.Failure<EmployeeLicense>($"'{nameof(licenseNumber)}' cannot be null, empty or more than {MaxLength} characters.");
        }
        
        if (string.IsNullOrEmpty(issuedBy) || issuedBy.Length > MaxLength)
        {
            return Result.Failure<EmployeeLicense>($"'{nameof(issuedBy)}' cannot be null, empty or more than {MaxLength} characters.");
        }
        
        if (validUntil.HasValue && validUntil.Value < issuedAt)
        {
            return Result.Failure<EmployeeLicense>(
                $"'RemovedAt' cannot be earlier than 'AssignedAt'.");
        }
        
        if (issuedAt.CompareTo(MaxDeltaDate) > 0 || issuedAt.CompareTo(MinDeltaDate) < 0)
        {
            return Result.Failure<EmployeeLicense>(
                $"'{nameof(issuedAt)}' is out of range. Must be between {MinDeltaDate.ToString("yyyy-MM-dd")} and {MaxDeltaDate.ToString("yyyy-MM-dd")}.");
        }
        
        if (validUntil.HasValue)
        {
            if (validUntil.Value.CompareTo(MaxDeltaDate) > 0 || validUntil.Value.CompareTo(MinDeltaDate) < 0)
            {
                return Result.Failure<EmployeeLicense>(
                    $"'{nameof(validUntil)}' is out of range. Must be between {MinDeltaDate:yyyy-MM-dd} and {MaxDeltaDate:yyyy-MM-dd}.");
            }
        }
        
        var employeeLicense = new EmployeeLicense(
            id, 
            employeeId, 
            licenseNumber, 
            issuedBy, 
            issuedAt, 
            validUntil);
        
        return Result.Success(employeeLicense);
    }
}