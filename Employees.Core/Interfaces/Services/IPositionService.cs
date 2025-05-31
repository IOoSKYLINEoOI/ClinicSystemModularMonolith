using CSharpFunctionalExtensions;
using Employees.Core.Enums;

namespace Employees.Core.Interfaces.Services;

public interface IPositionService
{
    Task<Result<HashSet<Position>>> GetEmployeePositions(Guid id);
}