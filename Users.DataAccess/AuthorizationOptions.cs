namespace Users.Domain;

public class AuthorizationOptions
{
    public RolePermissions[] RolePermissions { get; set; } = Array.Empty<RolePermissions>();
}

public class RolePermissions
{
    public string Role { get; set; } = string.Empty;
    public string[] Permissions { get; set; } = Array.Empty<string>();
}