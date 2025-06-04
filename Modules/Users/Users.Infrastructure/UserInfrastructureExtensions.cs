using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Interfaces.Authentication;
using Users.Infrastructure.Authentication;

namespace Users.Infrastructure;

public static class UserInfrastructureExtensions
{
    public static IServiceCollection AddUserInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddSingleton<IAuthorizationPolicyProvider, PolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}