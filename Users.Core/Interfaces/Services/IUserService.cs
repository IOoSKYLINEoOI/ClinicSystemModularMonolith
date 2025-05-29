using CSharpFunctionalExtensions;
using Users.Core.Enums;
using Users.Core.Models;
using Users.Core.ValueObjects;

namespace Users.Core.Interfaces.Services;

public interface IUserService
{
    Task<Result> Register(
        string firstName,
        string lastName,
        string? fatherName,
        DateOnly dateOfBirth,
        Gender gender,
        Email email,
        PhoneNumber phoneNumber,
        string password);

    Task<Result<string>> Login(Email email, string password);

    Task<Result> Update(
        Guid id,
        string firstName,
        string lastName,
        string? fatherName,
        DateOnly dateOfBirth,
        Email email,
        PhoneNumber phoneNumber);

    Task<Result<User>> GetByUserId(Guid id);

    Task<Result> ChangePassword(Guid userId, string currentPassword, string newPassword);
}