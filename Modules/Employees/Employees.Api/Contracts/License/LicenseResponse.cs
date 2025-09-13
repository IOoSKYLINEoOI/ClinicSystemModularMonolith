namespace Employees.Api.Contracts.License;

public record LicenseResponse(
    Guid Id,
    Guid EmployeeId,
    string LicenseNumber,
    string IssuedBy,
    DateOnly IssuedAt,
    DateOnly? ValidUntil);