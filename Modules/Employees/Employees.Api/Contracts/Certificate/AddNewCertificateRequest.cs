using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.Certificate;

public record AddNewCertificateRequest(
    [Required] Guid EmployeeId,
    [Required] string Name,
    [Required] string IssuedBy,
    [Required] DateOnly IssuedAt,
    DateOnly? ValidUntil);
