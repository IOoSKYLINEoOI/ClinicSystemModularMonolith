using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Users.Domain.Configuration;
using Users.Domain.Entities;

namespace Users.Domain;

public class UserDbContext(
    DbContextOptions<UserDbContext> options,
    IOptions<AuthorizationOptions> authOptions
) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserProfileEntity> UserProfiles { get; set; }
    
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<PermissionEntity> Permissions { get; set; }
    
    public DbSet<UserRoleEntity> UserRoles { get; set; } 
    public DbSet<RolePermissionEntity> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
    }
}