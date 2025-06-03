namespace Employees.Api.Contracts.Certificate;

public record CertificateResponse(
    Guid Id,
    Guid EmployeeId,
    string Name,
    string IssuedBy,
    DateOnly IssuedAt,
    DateOnly? ValidUntil);
