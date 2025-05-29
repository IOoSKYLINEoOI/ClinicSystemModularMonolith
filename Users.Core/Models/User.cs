using CSharpFunctionalExtensions;
using Users.Core.Enums;
using Users.Core.ValueObjects;

namespace Users.Core.Models;

public class User
{
    public const int MaxLength = 100;
    public static readonly DateOnly MaxDate = DateOnly.FromDateTime(DateTime.Now);
    public static readonly DateOnly MinDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-150));

    private User(
        Guid id,
        string firstName,
        string lastName,
        string? fatherName,
        DateOnly dateOfBirth,
        Gender gender,
        Email email,
        PhoneNumber phoneNumber,
        string passwordHash,
        bool isActive)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        FatherName = fatherName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Email = email;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        IsActive = isActive;
    }

    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string? FatherName { get; }
    public DateOnly DateOfBirth { get; }
    public Gender Gender { get; }
    public  Email Email { get; }
    public PhoneNumber PhoneNumber { get; }
    public string PasswordHash { get; }
    public bool IsActive { get; }
    
    public static Result<User> Create(
        Guid id,
        string firstName,
        string lastName,
        string? fatherName,
        DateOnly dateOfBirth,
        Gender gender,
        Email email,
        PhoneNumber phoneNumber,
        string passwordHash,
        bool isActive)
    {
        if (string.IsNullOrEmpty(firstName) || firstName.Length > MaxLength)
        {
            return Result.Failure<User>($"'{nameof(firstName)}' cannot be null, empty or more than {MaxLength} characters.");
        }
        if (string.IsNullOrEmpty(lastName) || lastName.Length > MaxLength)
        {
            return Result.Failure<User>($"'{nameof(lastName)}' cannot be null, empty or more than {MaxLength} characters.");
        }
        if (!string.IsNullOrEmpty(fatherName) && fatherName.Length > MaxLength)
        {
            return Result.Failure<User>($"'{nameof(fatherName)}' cannot be more than {MaxLength} characters.");
        }
        if (dateOfBirth.CompareTo(MaxDate) > 0 || dateOfBirth.CompareTo(MinDate) < 0)
        {
            return Result.Failure<User>($"'{nameof(dateOfBirth)}' is out of range. Must be between {MinDate.ToString("yyyy-MM-dd")} and {MaxDate.ToString("yyyy-MM-dd")}.");
        }

        var user = new User(
            id,
            firstName,
            lastName,
            fatherName,
            dateOfBirth,
            gender,
            email,
            phoneNumber,
            passwordHash,
            isActive);

        return Result.Success(user);
    }
}
