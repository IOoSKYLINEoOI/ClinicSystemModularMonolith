using System.ComponentModel.DataAnnotations;

namespace Patients.Api.Contracts.Patient;

public record UpdatePatientRequest(
    [Required] Guid Id,
    [Required] UpdateBloodProfileRequest UpdateBloodProfileRequest);
