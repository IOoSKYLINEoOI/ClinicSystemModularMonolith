using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Core.Interfaces.Repositories;
using Users.Domain.Repositories;

namespace Users.Domain;

public static class UserPersistenceExtensions
{
    public static IServiceCollection AddUserPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ClinicModularContextPostgreSQL")
                                 ?? throw new InvalidOperationException("Connection string 'ClinicModularContextPostgreSQL' not found.")));
        
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IUsersRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        
        return services;
    }
}