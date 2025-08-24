namespace Patients.DataAccess.Entities;

public class QuestionnaireEntity
{
    public Guid Id { get; set; }
    
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public bool? IsSmoker { get; set; }
    public bool? IsAlcoholConsumer { get; set; }
    public double? HeightCm { get; set; }
    public double? WeightKg { get; set; }
    
    public Guid PatientId { get; set; }
    public PatientEntity Patient { get; set; } = null!;
    
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}