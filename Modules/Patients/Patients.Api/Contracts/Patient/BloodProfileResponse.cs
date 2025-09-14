namespace Patients.Api.Contracts.PatientContracts;

public record BloodProfileResponse(
    string Type,
    string Rh,
    string Kell);
