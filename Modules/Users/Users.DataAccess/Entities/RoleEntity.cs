namespace Users.Domain.Entities;

public class RoleEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<PermissionEntity> Permissions { get; set; } = new List<PermissionEntity>();
    public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();

}