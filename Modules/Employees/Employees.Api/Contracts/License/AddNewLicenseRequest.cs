using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.License;

public record AddNewLicenseRequest(
    [Required] Guid EmployeeId,
    [Required] string LicenseNumber,
    [Required] string IssuedBy,
    [Required] DateOnly IssuedAt,
    DateOnly? ValidUntil);
