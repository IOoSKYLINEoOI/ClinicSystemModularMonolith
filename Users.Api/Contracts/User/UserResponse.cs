using Users.Core.Enums;
using Users.Core.ValueObjects;

namespace Users.Api.Contracts.User;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? FatherName,
    DateOnly DateOfBirth,
    Gender Gender,
    string Email,
    string PhoneNumber);