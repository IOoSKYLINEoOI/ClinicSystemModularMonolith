using Microsoft.EntityFrameworkCore;
using Users.Core.Enums;
using Users.Core.Interfaces.Repositories;
using Users.Domain.Entities;

namespace Users.Domain.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly UserDbContext _context;

    public UserRoleRepository(UserDbContext context)
    {
        _context = context;
    }
    
    public async Task AssignRoleToUserAsync(Guid userId, int roleId)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

        if (user is null)
            throw new InvalidOperationException("User not found or deleted.");

        var roleExists = await _context.Roles
            .AnyAsync(r => r.Id == roleId);

        if (!roleExists)
            throw new InvalidOperationException("Role not found.");

        var alreadyAssigned = user.UserRoles
            .Any(ur => ur.RoleId == roleId);

        if (alreadyAssigned)
            throw new InvalidOperationException("User already has this role.");

        var userRole = new UserRoleEntity
        {
            UserId = userId,
            RoleId = roleId
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
    }

    public Task<bool> HasRoleAsync(Guid userId, int roleId)
    {
        return _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
    
    public async Task<List<Role>> GetUserRolesAsync(Guid userId)
    {
        var roles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => (Role)ur.Role.Id) 
            .ToListAsync();

        return roles;
    }
}