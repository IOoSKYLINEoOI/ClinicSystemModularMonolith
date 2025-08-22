using Microsoft.EntityFrameworkCore;
using Patient.Core.Models;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess.Repositories;

public class MedicalRecordRepository
{
    private readonly PatientDbContext _context;

    public MedicalRecordRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task Add(MedicalRecord medicalRecord)
    {
        var medicalRecordEntity = MapToEntity(medicalRecord);

        _context.MedicalRecords.Add(medicalRecordEntity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(MedicalRecord medicalRecord)
    {
        var medicalRecordEntity = await _context.MedicalRecords.FirstOrDefaultAsync(c => c.Id == medicalRecord.Id);
        if (medicalRecordEntity == null)
            throw new InvalidOperationException("MedicalRecord not found");

        medicalRecordEntity.PatientId = medicalRecord.PatientId;
        medicalRecordEntity.EmployeeId = medicalRecord.EmployeeId;
        medicalRecordEntity.RecordDate = medicalRecord.RecordDate;
        medicalRecordEntity.Diagnosis = medicalRecord.Diagnosis;
        medicalRecordEntity.DiagnosisCode = medicalRecord.DiagnosisCode;
        medicalRecordEntity.Notes = medicalRecord.Notes;
        medicalRecordEntity.AppointmentId = medicalRecord.AppointmentId;
        medicalRecordEntity.CreatedAt = medicalRecord.CreatedAt;
        medicalRecordEntity.UpdatedAt = medicalRecord.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var medicalRecordEntity = await _context.MedicalRecords.FirstOrDefaultAsync(c => c.Id == id);
        if (medicalRecordEntity == null)
            throw new InvalidOperationException("MedicalRecord not found");

        _context.MedicalRecords.Remove(medicalRecordEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<MedicalRecord?> GetById(Guid id)
    {
        var medicalRecordEntity = await _context.MedicalRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return medicalRecordEntity == null ? null : MapToDomain(medicalRecordEntity);
    }

    public async Task<List<MedicalRecord>> GetByPatientId(Guid patientId)
    {
        var medicalRecordEntities = await _context.MedicalRecords
            .AsNoTracking()
            .Where(c => c.PatientId == patientId)
            .ToListAsync();

        return medicalRecordEntities.Select(MapToDomain).ToList();
    }

    private MedicalRecordEntity MapToEntity(MedicalRecord medicalRecord) => new MedicalRecordEntity()
    {
        Id = medicalRecord.Id,
        PatientId = medicalRecord.PatientId,
        EmployeeId = medicalRecord.EmployeeId,
        RecordDate = medicalRecord.RecordDate,
        Diagnosis = medicalRecord.Diagnosis,
        DiagnosisCode = medicalRecord.DiagnosisCode,
        Notes = medicalRecord.Notes,
        AppointmentId = medicalRecord.AppointmentId,
        CreatedAt = medicalRecord.CreatedAt,
        UpdatedAt = medicalRecord.UpdatedAt
    };

    private MedicalRecord MapToDomain(MedicalRecordEntity medicalRecordEntity) => MedicalRecord.FromPersistence(
        medicalRecordEntity.Id,
        medicalRecordEntity.PatientId,
        medicalRecordEntity.EmployeeId,
        medicalRecordEntity.RecordDate,
        medicalRecordEntity.Diagnosis,
        medicalRecordEntity.DiagnosisCode,
        medicalRecordEntity.Notes,
        medicalRecordEntity.AppointmentId,
        medicalRecordEntity.CreatedAt,
        medicalRecordEntity.UpdatedAt);
}