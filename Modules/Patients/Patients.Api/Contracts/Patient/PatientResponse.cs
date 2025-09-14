using Patients.Api.Contracts.PatientContracts;

namespace Patients.Api.Contracts.Patient;

public record PatientResponse(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    BloodProfileResponse? BloodProfile);

