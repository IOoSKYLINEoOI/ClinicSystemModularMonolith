using Employees.Core.Interfaces.Repositories;
using Employees.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.DataAccess;

public static class EmployeePersistenceExtensions
{
    public static IServiceCollection AddEmployeePersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<EmployeeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ClinicModularContextPostgreSQL")
                              ?? throw new InvalidOperationException(
                                  "Connection string 'ClinicModularContextPostgreSQL' not found.")));
        
        services.AddScoped<IEmployeeAssignmentRepository, EmployeeAssignmentRepository>();
        services.AddScoped<IEmployeeCertificateRepository, EmployeeCertificateRepository>();
        services.AddScoped<IEmployeeLicenseRepository, EmployeeLicenseRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        
        return services;
    }
}