using Users.Core.Enums;
using Users.Core.ValueObjects;

namespace Users.Api.Contracts.User;

public record UserResponse(
    Guid id,
    string firstName,
    string lastName,
    string? fatherName,
    DateOnly dateOfBirth,
    Gender gender,
    string email,
    string phoneNumber);