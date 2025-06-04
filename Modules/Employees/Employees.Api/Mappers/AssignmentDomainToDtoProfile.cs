using AutoMapper;
using Employees.Api.Contracts.Assignment;
using Employees.Core.Models;

namespace Employees.Api.Mappers;

public class AssignmentDomainToDtoProfile : Profile
{
    public AssignmentDomainToDtoProfile()
    {
        CreateMap<EmployeeAssignment, AssignmentResponse>();
    }
}