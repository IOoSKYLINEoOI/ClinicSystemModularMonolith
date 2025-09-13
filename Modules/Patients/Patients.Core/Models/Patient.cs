using CSharpFunctionalExtensions;
using Patients.Core.ValueObjects;

namespace Patients.Core.Models;

public class Patient
{
    private const int MaxLengthString = 255;
    
    private Patient(
        Guid id,
        Guid userId,
        DateTime createdAt,
        DateTime updatedAt,
        BloodProfile? bloodProfile)
    {
        Id = id;
        UserId = userId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        BloodProfile = bloodProfile;
    }
    
    public Guid Id { get; }
    public Guid UserId { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public BloodProfile? BloodProfile { get; }
    

    public static Result<Patient> Create(
        Guid id,
        Guid userId,
        DateTime createdAt,
        DateTime updatedAt,
        BloodProfile? bloodProfile)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Patient>("UserId не может быть пустым GUID.");
        
        var patient = new Patient(
            id,
            userId,
            createdAt,
            updatedAt,
            bloodProfile);

        return Result.Success(patient);
    }

    public static Patient FromPersistence(
        Guid id,
        Guid userId,
        DateTime createdAt,
        DateTime updatedAt,
        BloodProfile? bloodProfile)
    {
        return new Patient(
            id,
            userId,
            createdAt,
            updatedAt,
            bloodProfile);
    }
}

