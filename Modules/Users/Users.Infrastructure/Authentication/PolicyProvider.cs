
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Users.Core.Enums;
using Users.Domain;
using AuthorizationOptions = Microsoft.AspNetCore.Authorization.AuthorizationOptions;

//требует переработки. Работа через сервисы
namespace Users.Infrastructure.Authentication;
public class PolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConcurrentDictionary<string, AuthorizationPolicy> _policiesCache = new();

    public PolicyProvider(IServiceScopeFactory serviceScopeFactory, IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (_policiesCache.TryGetValue(policyName, out var cachedPolicy))
            return cachedPolicy;

        // Динамически подгружаем права из БД
        using var scope = _serviceScopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        var permissionExists = await db.Permissions.AnyAsync(p => p.Name == policyName);
        if (!permissionExists)
            return null;

        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionRequirement(new[] { Enum.Parse<Permission>(policyName) }))
            .Build();

        _policiesCache.TryAdd(policyName, policy);
        return policy;
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        _fallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        _fallbackPolicyProvider.GetFallbackPolicyAsync();
}
