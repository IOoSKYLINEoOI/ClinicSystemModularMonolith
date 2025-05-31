using CSharpFunctionalExtensions;
using Employees.Core.Enums;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Models;
using Microsoft.EntityFrameworkCore;
using Position = Employees.Core.Models.Position;

namespace Employees.DataAccess.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly EmployeeDbContext _context;

    public PositionRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<Result<HashSet<Position>>> GetEmployeePositions(Guid employeeId)
    {
        var positions = await _context.EmployeeAssignments
            .AsNoTracking()
            .Where(ur => ur.EmployeeId == employeeId)
            .Select(p => p.PositionId)
            .ToListAsync();
        
        if (!positions.Any())
            return Result.Failure<HashSet<Position>>("No employee assignments found");

        return positions.ToHashSet();
    }

    public async Task<Result<List<Position>>> GetAllPosition()
    {
        var positionsEntityes = await _context.Positions
            .AsNoTracking()
            .ToListAsync();
        
        if(positionsEntityes.Count == 0)
            return Result.Failure<List<Position>>("No positions found");
        
        var positions = new List<Position>();

        foreach (var positionEntity in positionsEntityes)
        {
            var position = Position.Create(
                positionEntity.Id,
                positionEntity.Name);
            
            if(!position.IsSuccess)
                return Result.Failure<List<Position>>("Position not found");
            
            positions.Add(position.Value);
        }
        
        return positions;
    }
}