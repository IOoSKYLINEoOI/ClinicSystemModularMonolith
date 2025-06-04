namespace Employees.Api.Contracts.EmployeeCertificate;

public record CertificateFilterRequest(
    Guid? EmployeeId,
    string? Name,
    string? IssuedBy,
    DateOnly? IssuedFrom,
    DateOnly? IssuedTo,
    DateOnly? ValidUntilFrom,
    DateOnly? ValidUntilTo);
