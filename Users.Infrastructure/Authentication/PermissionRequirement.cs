using Microsoft.AspNetCore.Authorization;
using Users.Core.Enums;

namespace Users.Infrastructure.Authentication;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permission[] Permissions { get; set; } = new Permission[0];

    public PermissionRequirement(Permission[] permissions) => Permissions = permissions;
}