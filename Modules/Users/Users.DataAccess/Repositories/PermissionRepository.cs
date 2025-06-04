using Microsoft.EntityFrameworkCore;
using Users.Core.Enums;
using Users.Core.Interfaces.Repositories;

namespace Users.Domain.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly UserDbContext _context;

    public PermissionRepository(UserDbContext context)
    {
        _context = context;
    }
    
    public async Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId)
    {
        var permissions = await _context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.Permissions)
            .Select(p => (Permission)p.Id)
            .ToListAsync();

        return permissions.ToHashSet();
    }
}