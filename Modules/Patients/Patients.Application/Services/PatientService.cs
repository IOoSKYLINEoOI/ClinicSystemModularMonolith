using CSharpFunctionalExtensions;
using Patients.Application.Commands.Patient;
using Patients.Application.Interfaces.Services;
using Patients.Core.Interfaces.Repository;
using Patients.Core.Models;
using Patients.Core.ValueObjects;

namespace Patients.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result> AddPatient(Guid userId)
    {
        var patientResult = Patient.Create(
            id: Guid.NewGuid(),
            userId: userId,
            createdAt: DateTime.Now,
            updatedAt: DateTime.Now,
            bloodProfile: null);
        
        if(patientResult.IsFailure)
            return Result.Failure(patientResult.Error);

        await _patientRepository.Add(patientResult.Value);
        return Result.Success();
    }

    public async Task<Result> UpdatePatient(UpdatePatientCommand updatePatientCommand)
    {
        var  patient = await _patientRepository.GetById(updatePatientCommand.Id);
        if(patient == null)
            return Result.Failure($"Patient with ID {updatePatientCommand.Id} not found");
        
        var bloodProfileResult = BloodProfile.Create(
            updatePatientCommand.UpdateBloodProfileCommand.Type,
            updatePatientCommand.UpdateBloodProfileCommand.Rh,
            updatePatientCommand.UpdateBloodProfileCommand.Kell);
        if(bloodProfileResult.IsFailure)
            return Result.Failure(bloodProfileResult.Error);
        
        
        var patientUpdate = Patient.Create(
            patient.Id,
            patient.UserId,
            patient.CreatedAt,
            updatedAt: DateTime.Now,
            bloodProfile: bloodProfileResult.Value);
        if(patientUpdate.IsFailure)
            return Result.Failure(patientUpdate.Error);
        
        await _patientRepository.Update(patientUpdate.Value);
        
        return Result.Success();
    }

    public async Task<Result<Patient>> GetPatient(Guid id)
    {
        if (id == Guid.Empty)
            return Result.Failure<Patient>("Invalid patient ID");
        
        var patient = await _patientRepository.GetById(id);
        if(patient == null)
            return Result.Failure<Patient>($"Patient with ID {id} not found");
        
        return Result.Success(patient);
    }
    
}