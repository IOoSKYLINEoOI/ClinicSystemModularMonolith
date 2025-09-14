using Patients.Api.Contracts.Patient;
using Patients.Api.Contracts.PatientContracts;
using Patients.Application.Commands.Patient;
using Patients.Core.Enums;
using Patients.Core.ValueObjects;

namespace Patients.Api.Mappers;

public class BloodProfileMapper
{
    public static BloodProfileResponse FromDomain(BloodProfile bloodProfile) 
        => new BloodProfileResponse(bloodProfile.Type.ToString(), bloodProfile.Rh.ToString(), bloodProfile.Kell.ToString()); 
    
    public static UpdateBloodProfileCommand FromUpdateCommand(UpdateBloodProfileRequest updateBloodProfileRequest) 
        => new UpdateBloodProfileCommand(
            (BloodGroup)updateBloodProfileRequest.Type, 
            (RhFactor)updateBloodProfileRequest.Rh, 
            (KellFactor)updateBloodProfileRequest.Kell); 
}