using CSharpFunctionalExtensions;

namespace Appointments.Core.Models;

public class MedicalRecord
{
    private MedicalRecord(
        Guid id,
        Guid patientId,
        Guid employeeId,
        DateTime recordDate,
        string diagnosis,
        string? diagnosisCode,
        string? notes,
        Guid? appointmentId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        PatientId = patientId;
        EmployeeId = employeeId;
        RecordDate = recordDate;
        Diagnosis = diagnosis;
        DiagnosisCode = diagnosisCode;
        Notes = notes;
        AppointmentId = appointmentId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }
    public Guid PatientId { get; }
    public Guid EmployeeId { get; }
    public DateTime RecordDate { get; }
    public string Diagnosis { get; }
    public string? DiagnosisCode { get; }
    public string? Notes { get; }
    public Guid? AppointmentId { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }

    public static Result<MedicalRecord> Create(
        Guid id,
        Guid patientId,
        Guid employeeId,
        DateTime recordDate,
        string diagnosis,
        string? diagnosisCode,
        string? notes,
        Guid? appointmentId)
    {
        if (patientId == Guid.Empty || employeeId == Guid.Empty)
            return Result.Failure<MedicalRecord>("PatientId and EmployeeId must be non-empty.");

        if (string.IsNullOrWhiteSpace(diagnosis))
            return Result.Failure<MedicalRecord>("Diagnosis must be provided.");

        if (recordDate > DateTime.UtcNow.AddDays(1))
            return Result.Failure<MedicalRecord>("Record date cannot be in the future.");

        var now = DateTime.UtcNow;

        return Result.Success(new MedicalRecord(
            id,
            patientId,
            employeeId,
            recordDate,
            diagnosis.Trim(),
            diagnosisCode?.Trim(),
            notes?.Trim(),
            appointmentId,
            createdAt: now,
            updatedAt: now
        ));
    }

    public static MedicalRecord FromPersistence(
        Guid id,
        Guid patientId,
        Guid employeeId,
        DateTime recordDate,
        string diagnosis,
        string? diagnosisCode,
        string? notes,
        Guid? appointmentId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new MedicalRecord(
            id,
            patientId,
            employeeId,
            recordDate,
            diagnosis,
            diagnosisCode,
            notes,
            appointmentId,
            createdAt,
            updatedAt);
    }
}