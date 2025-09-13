namespace Employees.Api.Contracts.License;

public record LicenseFilterRequest(
    Guid? EmployeeId,
    string? LicenseNumber,
    string? IssuedBy,
    DateOnly? IssuedFrom,
    DateOnly? IssuedTo,
    DateOnly? ValidUntilFrom,
    DateOnly? ValidUntilTo);