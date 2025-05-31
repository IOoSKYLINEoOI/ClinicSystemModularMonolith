using CSharpFunctionalExtensions;

namespace Employees.Core.Models.Filter;

public class EmployeeCertificateFilter
{
    private EmployeeCertificateFilter(
        Guid? employeeId, 
        string? name, 
        string? issuedBy, 
        DateOnly? issuedFrom, 
        DateOnly? issuedTo, 
        DateOnly? validUntilFrom, 
        DateOnly? validUntilTo)
    {
        EmployeeId = employeeId;
        Name = name;
        IssuedBy = issuedBy;
        IssuedFrom = issuedFrom;
        IssuedTo = issuedTo;
        ValidUntilFrom = validUntilFrom;
        ValidUntilTo = validUntilTo;
    }

    public Guid? EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? IssuedBy { get; set; }
    public DateOnly? IssuedFrom { get; set; }
    public DateOnly? IssuedTo { get; set; }
    public DateOnly? ValidUntilFrom { get; set; }
    public DateOnly? ValidUntilTo { get; set; }

    public static Result<EmployeeCertificateFilter> Create(
        Guid? employeeId,
        string? name,
        string? issuedBy,
        DateOnly? issuedFrom,
        DateOnly? issuedTo,
        DateOnly? validUntilFrom,
        DateOnly? validUntilTo)
    {
        if (issuedFrom.HasValue && issuedTo.HasValue && issuedFrom > issuedTo)
            return Result.Failure<EmployeeCertificateFilter>(
                $"'IssuedFrom' can't be more than 'IssuedTo'.");

        if (validUntilFrom.HasValue && validUntilTo.HasValue && validUntilFrom > validUntilTo)
            return Result.Failure<EmployeeCertificateFilter>(
                $"'ValidUntilFrom' can't be more than 'ValidUntilTo'.");

        return Result.Success(new EmployeeCertificateFilter(
            employeeId,
            name,
            issuedBy,
            issuedFrom,
            issuedTo,
            validUntilFrom,
            validUntilTo));
    }
}