using Employees.Core.Enums;
using Employees.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly EmployeeDbContext _context;

    public PositionRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<HashSet<Position>> GetEmployeePositionsAsync(Guid employeeId)
    {
        var positions = await _context.EmployeeAssignments
            .AsNoTracking()
            .Where(ur => ur.EmployeeId == employeeId)
            .Select(p => (Position)p.PositionId)
            .ToListAsync();

        return positions.ToHashSet();
    }
}