using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.License;

public record UpdateLicenseRequest(
    [Required] Guid Id,
    [Required] string LicenseNumber,
    [Required] string IssuedBy,
    DateOnly? ValidUntil);
