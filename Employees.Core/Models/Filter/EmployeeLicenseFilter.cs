using CSharpFunctionalExtensions;

namespace Employees.Core.Models.Filter;

public class 
    EmployeeLicenseFilter
{
    private EmployeeLicenseFilter(
        Guid? employeeId, 
        string? licenseNumber, 
        string? issuedBy, 
        DateOnly? issuedFrom, 
        DateOnly? issuedTo, 
        DateOnly? validUntilFrom, 
        DateOnly? validUntilTo)
    {
        EmployeeId = employeeId;
        LicenseNumber = licenseNumber;
        IssuedBy = issuedBy;
        IssuedFrom = issuedFrom;
        IssuedTo = issuedTo;
        ValidUntilFrom = validUntilFrom;
        ValidUntilTo = validUntilTo;
    }

    public Guid? EmployeeId { get; set; }
    public string? LicenseNumber { get; set; }
    public string? IssuedBy { get; set; }
    public DateOnly? IssuedFrom { get; set; }
    public DateOnly? IssuedTo { get; set; }
    public DateOnly? ValidUntilFrom { get; set; }
    public DateOnly? ValidUntilTo { get; set; }

    public static Result<EmployeeLicenseFilter> Create(
        Guid? employeeId,
        string? licenseNumber,
        string? issuedBy,
        DateOnly? issuedFrom,
        DateOnly? issuedTo,
        DateOnly? validUntilFrom,
        DateOnly? validUntilTo)
    {
        if (issuedFrom.HasValue && issuedTo.HasValue && issuedFrom > issuedTo)
            return Result.Failure<EmployeeLicenseFilter>(
                $"'IssuedFrom' can't be more than 'IssuedTo'.");

        if (validUntilFrom.HasValue && validUntilTo.HasValue && validUntilFrom > validUntilTo)
            return Result.Failure<EmployeeLicenseFilter>(
                $"'ValidUntilFrom' can't be more than 'ValidUntilTo'.");
        
        return Result.Success(new EmployeeLicenseFilter(
            employeeId,
            licenseNumber,
            issuedBy,
            issuedFrom,
            issuedTo,
            validUntilFrom,
            validUntilTo));
    }
}