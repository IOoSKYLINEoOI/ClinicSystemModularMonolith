using AutoMapper;
using Employees.Api.Contracts.License;
using Employees.Core.Models;

namespace Employees.Api.Mappers;

public class EmployeeLicenseDomainToDtoProfile : Profile
{
    public EmployeeLicenseDomainToDtoProfile()
    {
        CreateMap<EmployeeLicense, LicenseResponse>();
    }
}