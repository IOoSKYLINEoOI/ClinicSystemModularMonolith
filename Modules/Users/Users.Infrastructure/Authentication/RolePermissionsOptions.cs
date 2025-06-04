namespace Users.Infrastructure.Authentication;

public class RolePermissionsOptions
{
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>(); 
}

public class RolePermission
{
    public required string Role { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();  
}

