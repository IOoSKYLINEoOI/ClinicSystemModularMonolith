namespace Patients.DataAccess.Entities;

public class MedicalRecordEntity
{
    public required Guid Id { get; set; }
    
    public required DateTime RecordDate { get; set; }  
    public required string Diagnosis { get; set; }     
    public string? Notes { get; set; }
    public string? DiagnosisCode { get; set; }
    public Guid? AppointmentId { get; set; }
    public required Guid EmployeeId { get; set; }        

    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    
    public required Guid PatientId { get; set; }   
    public required PatientEntity Patient { get; set; }
}