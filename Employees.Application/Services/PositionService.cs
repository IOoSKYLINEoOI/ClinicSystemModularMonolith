using CSharpFunctionalExtensions;
using Employees.Core.Enums;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Interfaces.Services;
using Employees.Core.Models;

namespace Employees.Application.Services;

public class PositionService : IPositionService
{
    private readonly IPositionRepository _positionRepository;

    public PositionService(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public async Task<Result<List<Position>>> GetAllPositions()
    {
        return await _positionRepository.GetAll();
    }

    public async Task<Result> AddPosition(string name)
    {
        var positionResult = Position.Create(name);
        if(positionResult.IsFailure)
            return positionResult;
        
        await _positionRepository.Add(positionResult.Value);
        
        return Result.Success();
    }

    public async Task<Result<Position>> GetPositionById(int id)
    {
        return await _positionRepository.GetById(id);
    }
}