using CSharpFunctionalExtensions;
using Employees.Core.Enums;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Interfaces.Services;

namespace Employees.Application.Services;

public class PositionService : IPositionService
{
    private readonly IPositionRepository _positionRepository;

    public PositionService(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public async Task<Result<HashSet<Position>>> GetEmployeePositions(Guid id)
    {
        return await _positionRepository.GetEmployeePositionsAsync(id);
    }
}