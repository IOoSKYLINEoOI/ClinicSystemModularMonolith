using Microsoft.EntityFrameworkCore;
using Users.Core.Enums;
using Users.Domain.Entities;

namespace Users.Domain.Seeders;

public static class RolePermissionSeeder
{
    public static async Task SeedAsync(UserDbContext context)
    {
        var permissionNames = Enum.GetNames(typeof(Permission));
        foreach (var name in permissionNames)
        {
            if (!await context.Permissions.AnyAsync(p => p.Name == name))
            {
                await context.Permissions.AddAsync(new PermissionEntity { Name = name });
            }
        }

        await context.SaveChangesAsync();
        
        var roleNames = Enum.GetNames(typeof(Role));
        foreach (var name in roleNames)
        {
            if (!await context.Roles.AnyAsync(r => r.Name == name))
            {
                await context.Roles.AddAsync(new RoleEntity { Name = name });
            }
        }

        await context.SaveChangesAsync();

        var permissions = await context.Permissions.ToListAsync();
        var roles = await context.Roles.ToListAsync();

        foreach (var role in roles)
        {
            var roleEnum = Enum.Parse<Role>(role.Name);

            List<string> assignedPermissions = roleEnum switch
            {
                Role.Admin => permissionNames.ToList(),
                Role.Doctor => new List<string>
                {
                    Permission.ReadReception.ToString(),
                    Permission.CreateReception.ToString(),
                    Permission.UpdateReception.ToString(),
                    Permission.ReadResult.ToString(),
                    Permission.CreateResult.ToString(),
                    Permission.UpdateResult.ToString()
                },
                Role.SeniorDoctor => new List<string>
                {
                    Permission.ReadReception.ToString(),
                    Permission.CreateReception.ToString(),
                    Permission.UpdateReception.ToString(),
                    Permission.ReadResult.ToString(),
                    Permission.CreateResult.ToString(),
                    Permission.UpdateResult.ToString(),
                    Permission.DeleteResult.ToString()
                },
                Role.User => new List<string>
                {
                    Permission.ReadReception.ToString(),
                    Permission.ReadResult.ToString()
                },
                _ => new List<string>()
            };

            foreach (var permName in assignedPermissions)
            {
                var permission = permissions.First(p => p.Name == permName);

                if (!await context.RolePermissions.AnyAsync(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id))
                {
                    await context.RolePermissions.AddAsync(new RolePermissionEntity
                    {
                        RoleId = role.Id,
                        PermissionId = permission.Id
                    });
                }
            }
        }

        await context.SaveChangesAsync();
    }
}
