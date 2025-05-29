using CSharpFunctionalExtensions;
using Users.Core.Enums;

namespace Users.Core.Interfaces.Services;

public interface IUserRoleService
{
    Task<Result> AssignRoleToUserAsync(Guid userId, Role role);
    Task<Result<List<Role>>> GetRolesAsync(Guid userId);
}