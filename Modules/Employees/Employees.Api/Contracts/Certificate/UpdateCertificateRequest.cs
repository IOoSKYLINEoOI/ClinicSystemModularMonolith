using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.EmployeeCertificate;

public record UpdateCertificateRequest(
    [Required] Guid Id,
    [Required] string Name,
    [Required] string IssuedBy,
    DateOnly? ValidUntil);
