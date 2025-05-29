using Employees.Core.Enums;

namespace Employees.Core.Interfaces.Repositories;

public interface IPositionRepository
{
    Task<HashSet<Position>> GetEmployeePositionsAsync(Guid employeeId);
}