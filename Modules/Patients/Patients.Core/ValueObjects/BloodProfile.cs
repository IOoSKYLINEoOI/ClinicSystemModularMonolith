using CSharpFunctionalExtensions;
using Patients.Core.Enums;

namespace Patients.Core.ValueObjects;

public class 
    BloodProfile : ValueObject<BloodProfile>
{
    private BloodProfile(BloodGroup type, RhFactor rh, KellFactor kell)
    {
        Type = type;
        Rh = rh;
        Kell = kell;
    }

    public BloodGroup Type { get; }
    public RhFactor Rh { get; }
    public KellFactor Kell { get; }

    public static Result<BloodProfile> Create(
        BloodGroup type, 
        RhFactor rh, 
        KellFactor kell)
    {
        if (!Enum.IsDefined(typeof(BloodGroup), type))
            return Result.Failure<BloodProfile>("Invalid blood group");
    
        if (!Enum.IsDefined(typeof(RhFactor), rh))
            return Result.Failure<BloodProfile>("Invalid Rh factor");
    
        if (!Enum.IsDefined(typeof(KellFactor), kell))
            return Result.Failure<BloodProfile>("Invalid Kell factor");
        
        var bloodProfile = new BloodProfile(type, rh, kell);

        return Result.Success(bloodProfile);
    }

    public static BloodProfile FromPersistence(
        BloodGroup type,
        RhFactor rh,
        KellFactor kell)
    {
        return new BloodProfile(type, rh, kell);
    }

    protected override bool EqualsCore(BloodProfile other)
    {
        return Type == other.Type
               && Rh == other.Rh
               && Kell == other.Kell;
    }

    protected override int GetHashCodeCore()
    {
        return HashCode.Combine(Type, Rh, Kell);
    }
}