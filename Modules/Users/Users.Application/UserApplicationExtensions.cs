using Microsoft.Extensions.DependencyInjection;
using Users.Application.Services;
using Users.Core.Interfaces.Services;

namespace Users.Application;

public static class UserApplicationExtensions
{
    public static IServiceCollection AddUserApplication(this IServiceCollection services)
    {
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
}