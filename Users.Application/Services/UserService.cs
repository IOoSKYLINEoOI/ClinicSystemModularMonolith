using CSharpFunctionalExtensions;
using Users.Application.Interfaces.Authentication;
using Users.Core.Enums;
using Users.Core.Interfaces.Repositories;
using Users.Core.Interfaces.Services;
using Users.Core.Models;
using Users.Core.ValueObjects;

namespace Users.Application.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserRoleService _userRoleService;

    public UserService(
        IUsersRepository usersRepository, 
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IUserRoleService userRoleService)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _userRoleService = userRoleService;
    }

    public async Task<Result> Register(
        string firstName,
        string lastName,
        string? fatherName,
        DateOnly dateOfBirth,
        Gender gender,
        Email email,
        PhoneNumber phoneNumber,
        string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        
        var userResult = User.Create(
            id: Guid.NewGuid(),
            firstName, 
            lastName, 
            fatherName, 
            dateOfBirth, 
            gender, 
            email, 
            phoneNumber,
            hashedPassword,
            true);
        
        if (userResult.IsFailure)
            return Result.Failure(userResult.Error);

        await _usersRepository.Add(userResult.Value);
        return Result.Success();
    }
    
    public async Task<Result<string>> Login(Email email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);

        var passwordValid = _passwordHasher.Verify(password, user.PasswordHash);
        if (!passwordValid)
            return Result.Failure<string>("Invalid password");

        var rolesResult = await _userRoleService.GetRolesAsync(user.Id);
        if (!rolesResult.IsSuccess)
            return Result.Failure<string>("Roles not found");

        var token = _jwtProvider.Generate(user, rolesResult.Value);

        return Result.Success(token);
    }

    public async Task<Result> Update(
        Guid id,
        string firstName,
        string lastName,
        string? fatherName,
        DateOnly dateOfBirth,
        Email email,
        PhoneNumber phoneNumber)
    {
        await _usersRepository.Update(id, firstName, lastName, fatherName, dateOfBirth, email, phoneNumber);
        
        return Result.Success();
    }
    
    public async Task<Result<User>> GetByUserId(Guid id)
    {
        var user = await _usersRepository.GetById(id);

        return Result.Success(user);
    }
    
    public async Task<Result> ChangePassword(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _usersRepository.GetById(userId);
        
        var passwordValid = _passwordHasher.Verify(currentPassword, user.PasswordHash);
        if (!passwordValid)
            return Result.Failure<string>("Invalid password");
        
        var newPasswordHash = _passwordHasher.Generate(newPassword);

        var updateResult = await _usersRepository.UpdatePassword(user.Id, newPasswordHash);
        if (!updateResult)
            return Result.Failure("Failed to update password");

        return Result.Success();
    }
}