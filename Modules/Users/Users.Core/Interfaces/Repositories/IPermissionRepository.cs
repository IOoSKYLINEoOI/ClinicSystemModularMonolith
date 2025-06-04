using Users.Core.Enums;

namespace Users.Core.Interfaces.Repositories;

public interface 
    IPermissionRepository
{
    Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId);
}