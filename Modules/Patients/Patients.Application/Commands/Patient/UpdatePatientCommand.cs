using Patients.Core.Enums;

namespace Patients.Application.Commands.Patient;

public record UpdatePatientCommand(
    Guid Id,
    UpdateBloodProfileCommand UpdateBloodProfileCommand);

public record UpdateBloodProfileCommand(
    BloodGroup Type,
    RhFactor Rh,
    KellFactor Kell);
