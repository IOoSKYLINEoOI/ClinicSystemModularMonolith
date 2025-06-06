using Microsoft.EntityFrameworkCore;
using Patient.Core.Enums;

namespace Patients.DataAccess.Entities;

public class PatientEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    // Анкетные данные
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public bool? IsSmoker { get; set; }
    public bool? IsAlcoholConsumer { get; set; }
    public double? HeightCm { get; set; }
    public double? WeightKg { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    
    public BloodProfile? BloodProfile { get; set; }
    
    public ICollection<MedicalRecordEntity> PatientMedicalRecords { get; set; } = new List<MedicalRecordEntity>();
    public ICollection<ContactEntity> PatientContacts { get; set; } = new List<ContactEntity>(); 
    public InsuranceEntity? PatientInsurance { get; set; }
}

[Owned]
public class BloodProfile
{
    public required BloodGroup Type { get; set; }
    public required RhFactor Rh { get; set; }
    public required KellFactor Kell { get; set; }
}