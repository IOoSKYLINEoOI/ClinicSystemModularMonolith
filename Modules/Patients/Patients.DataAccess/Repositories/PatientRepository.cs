using Microsoft.EntityFrameworkCore;
using Patients.Core.Interfaces.Repository;
using Patients.DataAccess.Entities;
using Patients.Core.Models;
using Patients.Core.ValueObjects;

namespace Patients.DataAccess.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _context;

    public PatientRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task Add(Patient patient)
    {
        var patientEntity = MapToEntity(patient);

        _context.Patients.Add(patientEntity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Patient patient)
    {
        var patientEntity = await _context.Patients.FirstOrDefaultAsync(c => c.Id == patient.Id);
        if(patientEntity == null)
            throw new InvalidOperationException("Patient not found");

        patientEntity.UpdatedAt = patient.UpdatedAt;
        if(patient.BloodProfile != null)
            patientEntity.BloodProfileEntity = BloodProfileMapToEntity(patient.BloodProfile);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var patientEntity = await _context.Patients.FirstOrDefaultAsync(c => c.Id == id);
        if(patientEntity == null)
            throw new InvalidOperationException("Patient not found");

        _context.Patients.Remove(patientEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Patient?> GetById(Guid id)
    {
        var patientEntity = await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return patientEntity == null ? null : MapToDomain(patientEntity);
    }

    private static PatientEntity MapToEntity(Patient patient) => new PatientEntity()
    {
        Id = patient.Id,
        UserId = patient.UserId,
        CreatedAt = patient.CreatedAt,
        UpdatedAt = patient.UpdatedAt,
        BloodProfileEntity = patient.BloodProfile != null ? BloodProfileMapToEntity(patient.BloodProfile) : null
    };

    private static Patient MapToDomain(PatientEntity patientEntity) => Patient.FromPersistence(
        patientEntity.Id,
        patientEntity.UserId,
        patientEntity.CreatedAt,
        patientEntity.UpdatedAt,
        patientEntity.BloodProfileEntity == null ? null : BloodProfileMapToDomain(patientEntity.BloodProfileEntity));

    private static BloodProfileEntity BloodProfileMapToEntity(BloodProfile bloodProfile) => new BloodProfileEntity()
    {
        Type = bloodProfile.Type,
        Rh = bloodProfile.Rh,
        Kell = bloodProfile.Kell
    };

    private static BloodProfile BloodProfileMapToDomain(BloodProfileEntity bloodProfileEntity) => BloodProfile.FromPersistence(
        bloodProfileEntity.Type,
        bloodProfileEntity.Rh,
        bloodProfileEntity.Kell);
}