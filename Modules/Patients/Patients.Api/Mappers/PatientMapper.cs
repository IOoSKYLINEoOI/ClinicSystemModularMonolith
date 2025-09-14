using Patients.Api.Contracts.Patient;
using Patients.Api.Contracts.PatientContracts;
using Patients.Application.Commands.Patient;
using Patients.Core.Models;

namespace Patients.Api.Mappers;

public class PatientMapper
{
    public static PatientResponse FromDomain(Patient patient) 
        => new PatientResponse(
            patient.Id,
            patient.UserId,
            patient.CreatedAt,
            patient.UpdatedAt,
            patient.BloodProfile == null ? null : BloodProfileMapper.FromDomain(patient.BloodProfile));

    public static UpdatePatientCommand FromUpdateCommand(UpdatePatientRequest updatePatientRequest)
        => new UpdatePatientCommand(
            updatePatientRequest.Id,
            BloodProfileMapper.FromUpdateCommand(updatePatientRequest.UpdateBloodProfileRequest));
}