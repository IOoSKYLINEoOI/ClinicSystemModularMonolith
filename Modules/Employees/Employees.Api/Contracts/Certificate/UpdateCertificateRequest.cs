using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.Certificate;

public record UpdateCertificateRequest(
    [Required] Guid Id,
    [Required] string Name,
    [Required] string IssuedBy,
    DateOnly? ValidUntil);
