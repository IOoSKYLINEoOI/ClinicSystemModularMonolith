using Users.Core.Models;
using Users.Core.ValueObjects;

namespace Users.Core.Interfaces.Repositories;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User> GetByEmail(Email email);

    Task Update(
        Guid id,
        string firstName,
        string lastName,
        string? fatherName,
        DateOnly dateOfBirth,
        Email email,
        PhoneNumber phoneNumber
    );

    Task<User> GetById(Guid id);

    Task<bool> UpdatePassword(Guid userId, string newPasswordHash);
}