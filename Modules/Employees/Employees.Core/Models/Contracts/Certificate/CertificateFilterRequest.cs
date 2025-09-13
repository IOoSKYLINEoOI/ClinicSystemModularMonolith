namespace Employees.Api.Contracts.Certificate;

public record CertificateFilterRequest(
    Guid? EmployeeId,
    string? Name,
    string? IssuedBy,
    DateOnly? IssuedFrom,
    DateOnly? IssuedTo,
    DateOnly? ValidUntilFrom,
    DateOnly? ValidUntilTo);
