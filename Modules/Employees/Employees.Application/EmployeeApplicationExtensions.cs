using Employees.Application.Services;
using Employees.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Application;

public static class EmployeeApplicationExtensions
{
    public static IServiceCollection AddEmployeeApplication(this IServiceCollection services)
    {
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<ILicenseService, LicenseService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IPositionService, PositionService>();

        return services;
    }
}