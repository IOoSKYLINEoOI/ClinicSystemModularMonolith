namespace Users.Domain.Entities;

public class UserRoleEntity
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public Guid? AssignedByUserId { get; set; }
    public bool IsActive { get; set; }
    
    public UserEntity User { get; set; } = null!;
    public RoleEntity Role { get; set; } = null!;
}