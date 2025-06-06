using CSharpFunctionalExtensions;
using Patient.Core.ValueObjects;

namespace Patient.Core.Models;

public class Patient
{
    private const int MaxLengthString = 255;
    
    private Patient(
        Guid id,
        Guid userId,
        string? allergies,
        string? currentMedications,
        bool? isSmoker,
        bool? isAlcoholConsumer,
        double? heightCm,
        double? weightKg,
        DateTime createdAt,
        DateTime updatedAt,
        BloodProfile? bloodProfile)
    {
        Id = id;
        UserId = userId;
        Allergies = allergies;
        CurrentMedications = currentMedications;
        IsSmoker = isSmoker;
        IsAlcoholConsumer = isAlcoholConsumer;
        HeightCm = heightCm;
        WeightKg = weightKg;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        BloodProfile = bloodProfile;
    }
    
    public Guid Id { get; }
    public Guid UserId { get; }
    public string? Allergies { get; }
    public string? CurrentMedications { get; }
    public bool? IsSmoker { get; }
    public bool? IsAlcoholConsumer { get; }
    public double? HeightCm { get; }
    public double? WeightKg { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public BloodProfile? BloodProfile { get; }
    

    public static Result<Patient> Create(
        Guid id,
        Guid userId,
        string? allergies,
        string? currentMedications,
        bool? isSmoker,
        bool? isAlcoholConsumer,
        double? heightCm,
        double? weightKg,
        DateTime createdAt,
        DateTime updatedAt,
        BloodProfile? bloodProfile)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Patient>("UserId не может быть пустым GUID.");
        
        if (allergies != null && allergies.Length > MaxLengthString)
        {
            return Result.Failure<Patient>($"'{nameof(allergies)}' more than {MaxLengthString} characters.");
        }
        
        if (currentMedications != null && currentMedications.Length > MaxLengthString)
        {
            return Result.Failure<Patient>($"'{nameof(currentMedications)}' more than {MaxLengthString} characters.");
        }

        if (heightCm.HasValue && (heightCm <= 0 || heightCm > 300))
            return Result.Failure<Patient>("Рост должен быть в диапазоне от 0 до 300 см.");

        if (weightKg.HasValue && (weightKg <= 0 || weightKg > 500))
            return Result.Failure<Patient>("Вес должен быть в диапазоне от 0 до 500 кг.");
        
        
        var patient = new Patient(
            id,
            userId,
            allergies,
            currentMedications,
            isSmoker,
            isAlcoholConsumer,
            heightCm,
            weightKg,
            createdAt,
            updatedAt,
            bloodProfile);

        return Result.Success(patient);
    }
}

