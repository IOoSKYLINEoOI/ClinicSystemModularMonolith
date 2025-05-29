using CSharpFunctionalExtensions;
using Users.Core.Enums;
using Users.Core.Interfaces.Repositories;
using Users.Core.Interfaces.Services;

namespace Users.Application.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository;

    public UserRoleService(IUserRoleRepository userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;
    }

    public async Task<Result> AssignRoleToUserAsync(Guid userId, Role role)
    {
        var roleId = (int)role;

        var alreadyAssigned = await _userRoleRepository.HasRoleAsync(userId, roleId);
        if (alreadyAssigned)
            return Result.Failure("User already has this role");

        var result = await TryAssignRoleAsync(userId, roleId);
        return result;
    }

    private async Task<Result> TryAssignRoleAsync(Guid userId, int roleId)
    {
        try
        {
            await _userRoleRepository.AssignRoleToUserAsync(userId, roleId);
            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure($"Failed assign role: {ex.Message}");
        }
        catch (Exception)
        {
            return Result.Failure("Internal error assigning role.");
        }
    }

    public async Task<Result<List<Role>>> GetRolesAsync(Guid userId)
    {
        var result = await _userRoleRepository.GetUserRolesAsync(userId);
        
        return Result.Success(result);
    }
}

