using System.ComponentModel.DataAnnotations;

namespace Patients.Api.Contracts.Patient;

public record UpdateBloodProfileRequest(
    [Required] int Type,
    [Required] int Rh,
    [Required] int Kell);
