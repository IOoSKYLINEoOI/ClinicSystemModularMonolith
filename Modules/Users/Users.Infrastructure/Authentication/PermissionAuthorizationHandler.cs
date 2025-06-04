using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Users.Core.Interfaces.Services;

namespace Users.Infrastructure.Authentication;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory, ILogger<PermissionAuthorizationHandler> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }


    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(x => x.Type == "id");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            _logger.LogWarning("User ID claim is missing or invalid.");
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        var userPermissions = await permissionService.GetUserPermissionsAsync(userId);
        
        if (!userPermissions.IsSuccess || userPermissions.Value == null)
        {
            _logger.LogWarning("Failed to retrieve permissions for user {UserId}.", userId);
            return;
        }
        
        if (userPermissions.Value.Intersect(requirement.Permissions).Any())
        {
            _logger.LogInformation("User {UserId} has the required permissions.", userId);
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning("User {UserId} does not have the required permissions.", userId);
        }
    }
}