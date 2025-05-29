using CSharpFunctionalExtensions;
using Users.Core.Enums;

namespace Users.Core.Interfaces.Services;

public interface IPermissionService
{
    Task<Result<HashSet<Permission>>> GetUserPermissionsAsync(Guid userId);
}