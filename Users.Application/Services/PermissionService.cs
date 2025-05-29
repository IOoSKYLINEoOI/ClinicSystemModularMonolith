using CSharpFunctionalExtensions;
using Users.Core.Enums;
using Users.Core.Interfaces.Repositories;
using Users.Core.Interfaces.Services;

namespace Users.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<HashSet<Permission>>> GetUserPermissionsAsync(Guid userId)
    {
        var result = await _permissionRepository.GetUserPermissionsAsync(userId);

        return result;
    }
}
