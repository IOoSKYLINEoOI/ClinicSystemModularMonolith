using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Models;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly EmployeeDbContext _context;

    public PositionRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Position>>> GetAll()
    {
        var positionsEntityes = await _context.Positions
            .AsNoTracking()
            .ToListAsync();
        
        if(positionsEntityes.Count == 0)
            return Result.Failure<List<Position>>("No positions found");
        
        var positions = new List<Position>();

        foreach (var positionEntity in positionsEntityes)
        {
            var position = Position.Load(
                positionEntity.Id,
                positionEntity.Name);
            
            if(!position.IsSuccess)
                return Result.Failure<List<Position>>("Position not found");
            
            positions.Add(position.Value);
        }
        
        return positions;
    }

    public async Task<Result> Add(Position position)
    {
        var positionEntity = new PositionEntity()
        {
            Id = position.Id,
            Name = position.Name
        };
        
        await _context.Positions.AddAsync(positionEntity);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<Position>> GetById(int id)
    {
        var positionEntity = await _context.Positions
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if(positionEntity == null)
            return Result.Failure<Position>("Position not found");
        
        var positionResult = Position.Load(
            positionEntity.Id,
            positionEntity.Name);
        
        if(!positionResult.IsSuccess)
            return Result.Failure<Position>("Position not found");
        
        return positionResult;
    }
}