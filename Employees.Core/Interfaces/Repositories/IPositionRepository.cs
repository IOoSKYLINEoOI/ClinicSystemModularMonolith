using CSharpFunctionalExtensions;
using Employees.Core.Enums;

namespace Employees.Core.Interfaces.Repositories;

public interface IPositionRepository
{
    Task<Result<HashSet<Position>>> GetEmployeePositionsAsync(Guid employeeId);
}