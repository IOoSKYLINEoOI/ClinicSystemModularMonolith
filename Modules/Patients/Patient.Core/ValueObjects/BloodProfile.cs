using CSharpFunctionalExtensions;
using Patient.Core.Enums;

namespace Patient.Core.ValueObjects;

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

    public static Result<BloodProfile> Create(BloodGroup type, RhFactor rh, KellFactor kell)
    {
        var bloodProfile = new BloodProfile(type, rh, kell);

        return Result.Success(bloodProfile);
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