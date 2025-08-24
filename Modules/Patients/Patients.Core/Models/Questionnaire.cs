using CSharpFunctionalExtensions;

namespace Patients.Core.Models;

public class Questionnaire
{
    private const int MaxLengthString = 255;
    
    private Questionnaire(
        Guid id,
        string? allergies,
        string? currentMedications,
        bool? isSmoker,
        bool? isAlcoholConsumer,
        double? heightCm,
        double? weightKg,
        Guid patientId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Allergies = allergies;
        CurrentMedications = currentMedications;
        IsSmoker = isSmoker;
        IsAlcoholConsumer = isAlcoholConsumer;
        HeightCm = heightCm;
        WeightKg = weightKg;
        PatientId = patientId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    public Guid Id { get; }
    public string? Allergies { get; }
    public string? CurrentMedications { get; }
    public bool? IsSmoker { get; }
    public bool? IsAlcoholConsumer { get; }
    public double? HeightCm { get; }
    public double? WeightKg { get; }
    public Guid PatientId { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    
    public static Result<Questionnaire> Create(
        Guid id,
        string? allergies,
        string? currentMedications,
        bool? isSmoker,
        bool? isAlcoholConsumer,
        double? heightCm,
        double? weightKg,
        Guid patientId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        if (patientId == Guid.Empty)
            return Result.Failure<Questionnaire>("PatientId не может быть пустым GUID.");
        
        if (allergies != null && allergies.Length > MaxLengthString)
        {
            return Result.Failure<Questionnaire>($"'{nameof(allergies)}' more than {MaxLengthString} characters.");
        }
        
        if (currentMedications != null && currentMedications.Length > MaxLengthString)
        {
            return Result.Failure<Questionnaire>($"'{nameof(currentMedications)}' more than {MaxLengthString} characters.");
        }

        if (heightCm.HasValue && (heightCm <= 0 || heightCm > 300))
            return Result.Failure<Questionnaire>("Рост должен быть в диапазоне от 0 до 300 см.");

        if (weightKg.HasValue && (weightKg <= 0 || weightKg > 500))
            return Result.Failure<Questionnaire>("Вес должен быть в диапазоне от 0 до 500 кг.");
        
        
        var questionnaire = new Questionnaire(
            id,
            allergies,
            currentMedications,
            isSmoker,
            isAlcoholConsumer,
            heightCm,
            weightKg,
            patientId,
            createdAt,
            updatedAt);

        return Result.Success(questionnaire);
    }
}