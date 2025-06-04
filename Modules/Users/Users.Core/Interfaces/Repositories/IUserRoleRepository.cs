using Users.Core.Enums;

namespace Users.Core.Interfaces.Repositories;

public interface IUserRoleRepository
{
    Task AssignRoleToUserAsync(Guid userId, int roleId);
    Task<bool> HasRoleAsync(Guid userId, int roleId);
    Task<List<Role>> GetUserRolesAsync(Guid userId);
}