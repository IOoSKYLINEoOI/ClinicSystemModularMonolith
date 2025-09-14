using System.ComponentModel.DataAnnotations;

namespace Patients.Api.Contracts.Patient;

public record AddNewPatientRequest(
    [Required] Guid UserId);
