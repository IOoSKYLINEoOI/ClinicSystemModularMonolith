using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Users.Core.ValueObjects;

public class PhoneNumber: ValueObject
{
    public string Value { get; }
    public bool IsValid { get; }
    public bool IsActual { get; }
    public bool IsConfirmed { get; }
    
    private PhoneNumber(string value, bool isValid, bool isActual, bool isComfirmed)
    {
        Value = value;
        IsValid = isValid;
        IsActual = isActual;
        IsConfirmed = isComfirmed;
    }
    
    public static Result<PhoneNumber> Create(string value, bool isValid, bool isActual, bool isComfirmed)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<PhoneNumber>("Phone number cannot be empty.");

        if (!Regex.IsMatch(value, @"^\+?[0-9]{10,15}$"))
            return Result.Failure<PhoneNumber>("Invalid phone number format.");

        return Result.Success(new PhoneNumber(value, isValid, isActual, isComfirmed));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}