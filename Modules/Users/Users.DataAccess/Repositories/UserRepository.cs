using Microsoft.EntityFrameworkCore;
using Users.Core.Enums;
using Users.Core.Interfaces.Repositories;
using Users.Core.Models;
using Users.Core.ValueObjects;
using Users.Domain.Entities;

namespace Users.Domain.Repositories;

public class UserRepository : IUsersRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        var roleEntity = await _context.Roles
                             .FirstOrDefaultAsync(r => r.Id == (int)Role.User)
                         ?? throw new InvalidOperationException("Role not found.");

        var userEntity = new UserEntity()
        {
            Id = user.Id,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PasswordHash = user.PasswordHash,
            
            Profile = new ProfileEntity()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.FatherName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender
            }
        };
        
        var userRoleEntity = new UserRoleEntity
        {
            User = userEntity,
            Role = roleEntity
        };

        userEntity.UserRoles = new List<UserRoleEntity> { userRoleEntity };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByEmail(Email email)
    {
        var userEntity = await _context.Users
                             .AsNoTracking().Include(userEntity => userEntity.Profile)
                             .FirstOrDefaultAsync(u => u.Email.Value == email.Value && u.IsActive)
            ?? throw new Exception("User not found or deleted.");

        var user = User.Create(
            userEntity.Id,
            userEntity.Profile.FirstName,
            userEntity.Profile.LastName,
            userEntity.Profile.MiddleName,
            userEntity.Profile.DateOfBirth,
            userEntity.Profile.Gender,
            userEntity.Email,
            userEntity.PhoneNumber,
            userEntity.PasswordHash,
            userEntity.IsActive);
        
        if (!user.IsSuccess)
            throw new InvalidOperationException(user.Error);
        
        return user.Value;
    }

    public async Task Update(
         Guid id,
         string firstName,
         string lastName,
         string? fatherName,
         DateOnly dateOfBirth,
         Email email,
         PhoneNumber phoneNumber
         )
    {
        var user = await _context.Users
                       .Include(u => u.Profile)
                       .FirstOrDefaultAsync(u => u.Id == id && u.IsActive)
                   ?? throw new InvalidOperationException("User not found or deleted.");

        user.Email = email;
        user.PhoneNumber = phoneNumber;

        user.Profile.FirstName = firstName;
        user.Profile.LastName = lastName;
        user.Profile.MiddleName = fatherName;
        user.Profile.DateOfBirth = dateOfBirth;

        await _context.SaveChangesAsync();
    }
    
    public async Task<User> GetById(Guid id)
    {
        var userEntity = await _context.Users
                             .AsNoTracking().Include(userEntity => userEntity.Profile)
                             .FirstOrDefaultAsync(u => u.Id == id && u.IsActive)
                         ?? throw new InvalidOperationException("User not found or deleted.");

        var user = User.Create(
            userEntity.Id,
            userEntity.Profile.FirstName,
            userEntity.Profile.LastName,
            userEntity.Profile.MiddleName,
            userEntity.Profile.DateOfBirth,
            userEntity.Profile.Gender,
            userEntity.Email,
            userEntity.PhoneNumber,
            userEntity.PasswordHash,
            userEntity.IsActive);
        
        if (!user.IsSuccess)
            throw new InvalidOperationException(user.Error);

        return user.Value;
    }
    
    public async Task<bool> UpdatePassword(Guid userId, string newPasswordHash)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new InvalidOperationException("User not found or deleted.");

        user.PasswordHash = newPasswordHash;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }
}